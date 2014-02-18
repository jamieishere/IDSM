using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDSM.Model;
using IDSM.Repository;
using IDSM.Repository.DTOs;
using IDSM;
using System.Transactions;
using System.Data.Common;
using AutoMapper;
using System.IO;
using CsvHelper;
using IDSM.ViewModel;

namespace IDSM.ServiceLayer
{
    public class Service: IService, IDisposable
    {
        private IUnitOfWork _unitOfWork;
       // private IUnitOfWorkFactory _unitOfWorkFactory;
        private IUserRepository _userRepository;
        private IUserTeamRepository _userTeamRepository;
        private IPlayerRepository _playerRepository;
        private IGameRepository _gameRepository;
        private IUserTeam_PlayerRepository _userTeamPlayerRepository;

        public Service(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            initialiseRepos();
        }

        private void initialiseRepos(){
            _userRepository = _userRepository ?? new UserRepository(_unitOfWork.Context);
            _userTeamRepository = _userTeamRepository ?? new UserTeamRepository(_unitOfWork.Context);
            _playerRepository = _playerRepository ?? new PlayerRepository(_unitOfWork.Context);
            _gameRepository = _gameRepository ?? new GameRepository(_unitOfWork.Context);
            _userTeamPlayerRepository = _userTeamPlayerRepository ?? new UserTeam_PlayerRepository(_unitOfWork.Context);
        }

        //public IUserRepository Users { get { return _userRepository; } }
        //public IUserTeamRepository UserTeams { get { return _userTeamRepository; } }
        //public IPlayerRepository Players { get { return _playerRepository; } }
        //public IGameRepository Games { get { return _gameRepository; } }
        //public IUserTeam_PlayerRepository UserTeamPlayers { get { return _userTeamPlayerRepository; } }

        #region METHODS USED IN GAME CONTROLLER

        /// GetAllGames
        /// Gets all Games
        /// </summary>
        /// <returns>IEnumerable<Game></returns>
        public IEnumerable<Game> GetAllGames()
        {
            var _games = _gameRepository.GetAllGames();
            return _games;
        }

        public OperationStatus CreateGame(int creatorId, string name)
        {
            Game game = new Game() { CreatorId = creatorId, Name = name };
            OperationStatus _opStatus = _gameRepository.Create(game);
            return _opStatus;
        }

        public OperationStatus StartGame(int gameId)
        {
            var _opStatus = new OperationStatus { Status = true };
            try
            {
                List<UserTeam> _userTeams = GetAllUserTeamsForGame(gameId, "Id");
                if (_userTeams != null)
                {
                    _userTeams.Shuffle();
                    foreach (UserTeam _team in _userTeams)
                    {
                        _team.OrderPosition = _userTeams.IndexOf(_team);
                        SaveUserTeam(_team);
                    }

                    Game _game = GetGame(gameId);
                    _game.HasStarted = true;
                    DoUpdateGame(_game);
                }
            }
            catch (Exception exp)
            {
                _opStatus = OperationStatus.CreateFromException("Error starting game: " + exp.Message, exp);
            }
            return _opStatus;
        }

        public OperationStatus ResetGame(int gameId)
        {
            var _opStatus = new OperationStatus { Status = true };
            try
            {
                IEnumerable<UserTeam> _userTeams = GetAllUserTeamsForGame(gameId, "Id");
                if (_userTeams != null)
                {
                    foreach (UserTeam _team in _userTeams)
                    {
                        //cascading delete setup for UserTeam - UserTeam_Players in IDSMContext
                        DeleteUserTeam(_team);
                    }

                    //reset all game properties to default
                    Game _game = GetGame(gameId);
                    _game.WinnerId = 0;
                    _game.HasEnded = false;
                    _game.HasStarted = false;
                    _game.CurrentOrderPosition = 0;
                    _opStatus = DoUpdateGame(_game);
                }
            }
            catch (Exception exp)
            {
                _opStatus = OperationStatus.CreateFromException("Error resetting game: " + exp.Message, exp);
            }
            return _opStatus;
        }

        public OperationStatus AddUserToGame(int userId, int gameId)
        {
            var _opStatus = new OperationStatus { Status = false };
            UserTeam ut = null;
            try
            {
                if (!TryGetUserTeam(userTeamId: 0, gameId: gameId, userId: (int)userId, userTeam: out ut))
                {
                    _opStatus = CreateUserTeam(userId, gameId);   
                    // should call Save in here so user never sees fail.  keep controller thinner.
                }
            }
            catch
            {
                //_opStatus = OperationStatus.CreateFromException("Error AddUserToGame: " + exp.Message, exp);
            }
            return _opStatus;
        }

        #endregion

        #region METHODS USED IN VIEW PLAYERS CONTROLLER
        // think this is used by viewplayerscontroller - 
        public SearchViewModel GetUserTeamViewModel(int userTeamId, string footballClub, string searchString)
        {
            // setup message to be displayed to User once they have added their chosen player
            string _addedPlayerMessage = "Current player is {0}.  There are {1} turns left before your go.";
            string _tmpActiveUserName;
            int _tmpTurnsLeft = 0;
            UserTeam _userTeam = null;
            UserProfile _user = null;
            IEnumerable<UserTeam_Player> _playersPickedForThisTeam = null;
            IEnumerable<PlayerDto> _playersNotPickedForAnyTeam = new List<PlayerDto>();
            Game _game = null;

            // get this UserTeam, User and Game
            if (!TryGetUserTeam(out _userTeam, userTeamId: userTeamId))
                //return RedirectToAction("ApplicationError", "Error");
                throw new ApplicationException();
            if (!TryGetUser(out _user, _userTeam.UserId))
                //return RedirectToAction("ApplicationError", "Error");
                throw new ApplicationException();
            if (!TryGetGame(out _game, _userTeam.GameId))
                //return RedirectToAction("ApplicationError", "Error");
                throw new ApplicationException();
            if (_game.HasEnded) { _addedPlayerMessage = "The game has ended."; }
            else
            {
                _playersPickedForThisTeam = GetAllChosenUserTeamPlayersForTeam(_userTeam.Id);
                _playersNotPickedForAnyTeam = GetPlayersNotPickedForAnyTeam(_game.Id, footballClub, searchString);

                List<UserTeam> _userTeamsForGame = GetAllUserTeamsForGame(_game.Id, "Id");

                if (_userTeam.OrderPosition != _game.CurrentOrderPosition)
                {
                    UserTeam _activeUt = GetUserTeamByOrderPosition(_game.CurrentOrderPosition, _game.Id);
                    _tmpActiveUserName = GetUser(_activeUt.UserId).UserName;

                    switch (_activeUt.OrderPosition > _userTeam.OrderPosition)
                    {
                        case true:
                            _tmpTurnsLeft = (_userTeam.OrderPosition == _userTeamsForGame.Count) ? 1 : _activeUt.OrderPosition - _userTeam.OrderPosition;
                            break;
                        case false:
                            _tmpTurnsLeft = (_userTeam.OrderPosition == _userTeamsForGame.Count) ? 1 : _userTeam.OrderPosition - _activeUt.OrderPosition;
                            break;
                    }
                    _addedPlayerMessage = String.Format(_addedPlayerMessage, _tmpActiveUserName, _tmpTurnsLeft);
                }
            }

            return new SearchViewModel() { Players_SearchedFor = _playersNotPickedForAnyTeam, Players_Chosen = _playersPickedForThisTeam, GameId = _userTeam.GameId, GameName = _game.Name, GameCurrentOrderPosition = _game.CurrentOrderPosition, UserTeamId = _userTeam.Id, UserName = _user.UserName, UserTeamOrderPosition = _userTeam.OrderPosition, AddedPlayerMessage = _addedPlayerMessage };
        }
        #endregion

        #region GAMES METHODS NOT IN CONTROLLER

        /// <summary>
        /// GetGame
        /// Get single Game by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Game</returns>
        public Game GetGame(int id)
        {
            var _game = _gameRepository.Get(s => s.Id == id);
            return _game;
        }

        /// <summary>
        /// TryGetGame
        /// Get single Game by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Game</returns>
        public Boolean TryGetGame(out Game game, int id)
        {
            game = GetGame(id);
            if (game == null) return false;
            return true;
        }

        public OperationStatus DoUpdateGame(Game game)
        {
            GameUpdateDTO _gameDto = new GameUpdateDTO();
            _gameDto = Mapper.Map(game, _gameDto);
            return _gameRepository.Update(_gameDto, g => g.Id == _gameDto.Id);
        }

        #endregion

        #region USER METHODS

        public UserProfile GetUser(int userId)
        {
            UserProfile _user = _userRepository.Get(x => x.UserId == userId);
            return _user;
        }

        public IEnumerable<UserProfile> GetAllUsers()
        {
            var up = _userRepository.GetList().ToList();
            return up;
        }

        public Boolean TryGetUser(out UserProfile userProfile, int userId)
        {
            userProfile = _userRepository.Get(u => u.UserId == userId);
            if (userProfile == null) return false;
            return true;
        }
        #endregion

        #region USERTEAM METHODS
        public IEnumerable<UserTeam> GetAllUserTeams()
        {
            var ut = _userTeamRepository.GetList().ToList();
            return ut;
        }

        public List<UserTeam> GetAllUserTeamsForGame(int gameId, string orderBy)
        {
            List<UserTeam> ut = new List<UserTeam>();
            switch (orderBy)
            {
                case "Id":
                    ut = _userTeamRepository.GetList(t => t.GameId == gameId, o => o.Id).ToList();
                   // ut = DataContext.UserTeams.Where(t => t.GameId == gameId).OrderBy(o => o.Id).ToList();
                    break;
                case "OrderPosition":
                    ut = _userTeamRepository.GetList(t => t.GameId == gameId, o => o.OrderPosition).ToList();
                    //ut = DataContext.UserTeams.Where(t => t.GameId == gameId).OrderBy(o => o.OrderPosition).ToList();
                    break;
            }
            return ut;
        }


        /// <summary>
        /// TryGet a UserTeam by UserTeamId, OR with GameID & UserID
        /// </summary>
        /// <param name="userTeamId"></param>
        /// <param name="gameId"></param>
        /// <param name="userId"></param>
        /// <param name="userTeam"></param>
        /// <returns>OperationStatus(Status=True) & UserTeam or OperationStatus(Status=False) & null</returns>
        public Boolean TryGetUserTeam(out UserTeam userTeam, int userTeamId = 0, int gameId = 0, int userId = 0)
        {
            userTeam = GetUserTeam(userTeamId, gameId, userId);
            if (userTeam == null) return false;
            return true;
        }

        /// <summary>
        /// Get a UserTeam by UserTeamId, OR with GameID & UserID
        /// </summary>
        /// <param name="userTeamId"></param>
        /// <param name="gameId"></param>
        /// <param name="userId"></param>
        /// <returns>UserTeam or null</returns>
        public UserTeam GetUserTeam(int userTeamId = 0, int gameId = 0, int userId = 0)
        {
            UserTeam ut = (userTeamId != 0) ?
                _userTeamRepository.Get(s => s.Id == userTeamId, u => u.UserTeam_Players) :
                _userTeamRepository.Get(s => s.GameId == gameId && s.UserId == userId, u => u.UserTeam_Players);
            return ut;
        }

        /// <summary>
        /// Gets the UserTeam for a Game by the UserTeam's OrderPosition
        /// </summary>
        /// <param name="orderposition"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public UserTeam GetUserTeamByOrderPosition(int orderPosition, int gameId)
        {
            //var ut = DataContext.UserTeams.Include("User").SingleOrDefault(s => s.OrderPosition == orderPosition && s.GameId==gameId);
            var ut = _userTeamRepository.Get(s => s.OrderPosition == orderPosition && s.GameId == gameId, u => u.User);
            return ut;
        }

        /// <summary>
        /// GetAllChosenPlayerIdsForGame
        /// Gets int array of all the Ids of all the UserTeam_Players currently selected by UserTeams for specific Game
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns>int[]</returns>
        /// <remarks>
        /// Returning an array gives marginal performance benefit in this situation 
        /// http://stackoverflow.com/questions/434761/array-versus-listt-when-to-use-which
        /// </remarks>
        public int[] GetAllChosenUserTeamPlayerIdsForGame(int gameId) 
        {
            var _chosenPlayers = _userTeamPlayerRepository.GetList(p => p.GameId ==gameId, p => p.PlayerId).Select(x => x.PlayerId).ToArray();
            return _chosenPlayers;
        }

        /// <summary>
        /// GetAllChosenPlayersForGame
        /// Gets all the UserTeam_Players currently selected by UserTeams for specific Game
        /// </summary>
        /// <param name="gameid"></param>
        /// <returns>IEnumerable<UserTeam_Player></returns>
        public IEnumerable<UserTeam_Player> GetAllChosenUserTeamPlayersForGame(int gameid)
        {
            var _chosenPlayers = _userTeamPlayerRepository.GetList(p => p.GameId ==gameid, p => p.PlayerId).ToList();
            return _chosenPlayers;
        }

        /// <summary>
        /// GetAllChosenPlayersForUserTeam
        /// Gets all the UserTeam_Players currently selected by a specific UserTeam
        /// </summary>
        /// <param name="userTeamId"></param>
        /// <returns>IEnumerable<UserTeam_Player></returns>
        public IEnumerable<UserTeam_Player> GetAllChosenUserTeamPlayersForTeam(int userTeamId)
        {
            var _chosenPlayers = _userTeamPlayerRepository.GetList(p => p.UserTeamId == userTeamId, p => p.PlayerId).ToList();
            return _chosenPlayers;
        }

        // TODO:
        // Prevent duplicates being created.
        public OperationStatus SaveUserTeamPlayer(int userteamid, int gameid, int pixelposy, int pixelposx, int playerid)
        {
            UserTeam_Player player = new UserTeam_Player() { UserTeamId = userteamid, GameId = gameid, PixelPosX = pixelposx, PixelPosY = pixelposy, PlayerId = playerid };
            //var temp = DataContext.UserTeam_Players.Where(p => p.PlayerId == playerid && p.UserTeamId == userteamid).SingleOrDefault();
            var temp = _userTeamPlayerRepository.Get(p => p.PlayerId == playerid && p.UserTeamId == userteamid);

            // if userteam_player already exists, return true
            if (temp != null) return new OperationStatus { Status = true };

            try
            {
                _userTeamPlayerRepository.Create(player);
            }
            catch (Exception ex)
            {
                return OperationStatus.CreateFromException("Error - SaveUserTeamPlayer.", ex);
            }
            return new OperationStatus { Status = true };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userTeam"></param>
        /// <returns></returns>
        /// <remarks>
        /// toto:  DataContext.UserTeams.Remove(userTeam); can this cascade - delete all userteam players?  must set a foreign key.
        ///no, using remove like this doesn't work because the entity is not attached or something..
        /// need to better understand how deletion works....
        ///http://stackoverflow.com/questions/15637965/the-object-cannot-be-deleted-because-it-was-not-found-in-the-objectstatemanager
        ///http://stackoverflow.com/questions/1217052/entity-framework-delete-object-problem
        ///also chekc that the reason this does a cascade delete (deletes all the userteam players too), is beacause I set the cascade to true in IDSMContext (check if I remove it, if the cascade still happens)
        ///</remarks>
        public OperationStatus DeleteUserTeam(UserTeam userTeam)
        {
            _userTeamRepository.Delete(userTeam);
            return new OperationStatus { Status = true };
        }

        public OperationStatus SaveUserTeam(UserTeam team)
        {
            try
            {
                var ut = _userTeamRepository.Get(u => u.Id == team.Id);

                if (ut != null)
                {
                    ut.GameId = team.GameId;
                    ut.OrderPosition = team.OrderPosition;
                    ut.UserId = team.UserId;
                }
            }
            catch (Exception ex)
            {
                return OperationStatus.CreateFromException("Error updating userteam.", ex);
            }
            return new OperationStatus { Status = true };
        }


        public OperationStatus CreateUserTeam(int userid, int gameid)
        {
            var opStatus = new OperationStatus { Status = false };
            UserTeam ut = new UserTeam() { UserId = userid, GameId = gameid };
            try
            {
                _userTeamRepository.Create(ut);
            }
            catch (Exception ex)
            {
                return OperationStatus.CreateFromException("Error creating userteam.", ex);
            }

            return new OperationStatus { Status = true, OperationID = ut.Id };
        }
        #endregion

        #region PLAYERMETHODS
        public Boolean TryGetPlayer(out Player player, int playerId)
        {
            player = GetPlayer(playerId);
            if (player == null) return false;
            return true;
        }

        /// <summary>
        /// GetPlayer
        /// Gets a Player by Id
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns>Player</returns>
        public Player GetPlayer(int playerId)
        {
            Player _player = _playerRepository.Get(p => p.Id == playerId);
            return _player;
        }

        /// <summary>
        /// GetAllPlayers
        /// Gets all Players
        /// </summary>
        /// <returns>IEnumerable<Player></returns>
        public IEnumerable<Player> GetAllPlayers()
        {
            var _players = _playerRepository.GetList().ToList();
            return _players;
        }

        /// <summary>
        /// GetPlayersNotPickedForAnyTeam
        /// Gets all players based on search parameters (club & searchstring) that have not already been picked.
        /// </summary>
        /// <returns>IEnumerable<Player></returns>
        public IEnumerable<PlayerDto> GetPlayersNotPickedForAnyTeam(int gameId, string footballClub, string searchString)
        {
            IEnumerable<Player> _footballPlayers = null;
            int[] _chosenPlayerIds = null;

            _footballPlayers = GetAllPlayers(); 
            _chosenPlayerIds = GetAllChosenUserTeamPlayerIdsForGame(gameId);

            // map Player to DTO object (has extra properties)
            IEnumerable<PlayerDto> _footballPlayersDto = new List<PlayerDto>();
            Mapper.CreateMap<Player, PlayerDto>();
            _footballPlayersDto = Mapper.Map<IEnumerable<PlayerDto>>(_footballPlayers);

            //update player list, marking those already chosen
            foreach (PlayerDto p in _footballPlayersDto)
            {
                if (_chosenPlayerIds.Contains(p.Id)) { p.HasBeenChosen = true; }
            }

            //if passed, filter players by searchstring
            if (!String.IsNullOrEmpty(searchString)) 
                _footballPlayersDto = _footballPlayersDto.Where(s => s.Name.Contains(searchString));

            //if passed, filter players by club
            if (!String.IsNullOrEmpty(footballClub))
                _footballPlayersDto = _footballPlayersDto.Where(x => x.Club == footballClub);

            return _footballPlayersDto;
        }


        /// <summary>
        /// GetAllClubs
        /// Gets a distinct list of all the football clubs in the Player database
        /// </summary>
        /// <returns>IEnumerable<string></returns>
        public IEnumerable<string> GetAllClubs()
        {
            var clubLst = new List<string>();
            //var clubQry = from fp in DataContext.Players
            //              orderby fp.Club
            //              select fp.Club;
            var _clubs = _playerRepository.GetList().OrderBy(p => p.Club).Select(p => p.Club);
            clubLst.AddRange(_clubs.Distinct());

            return clubLst;
        }

        /// <summary>
        /// UploadPlayersCSV
        /// Takes a .csv file containing Player data in the correct model format, inserts into the database
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>OperationStatus</returns>
        public static OperationStatus UploadPlayersCSV(string filePath)
        {
            return ProcessCSVHelper(filePath, new IDSMContext());
        }

        /// <summary>
        /// ProcessCSVHelper
        /// Takes a .csv file containing Player data in the correct model format, inserts into the database
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="DataContext"></param>
        /// <returns>OperationStatus</returns>
        public static OperationStatus ProcessCSVHelper(string filePath, IDSMContext DataContext)
        {
            string Feedback = string.Empty;
            StreamReader srCSV = new StreamReader(filePath);
            CsvReader csvReader = new CsvReader(srCSV);

            // NOTE:
            //      'ID' error on CSV import is either is coming from this line, or the for each loop below.
            //       Temporarily fixed by adding an ID column to CSV
            List<Player> FootballPlayerList = new List<Player>();

            try
            {
                FootballPlayerList = new List<Player>(csvReader.GetRecords<Player>());
            }
            catch (Exception ex)
            {
                return OperationStatus.CreateFromException("Error reading from CSV.", ex);
            }

            try
            {
                foreach (Player m in FootballPlayerList)
                {
                    DataContext.Players.Add(m);
                }
                DataContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return OperationStatus.CreateFromException("Error saving players to DB from CSV.", ex);
            }

            srCSV.Dispose();
            csvReader.Dispose();

            return new OperationStatus { Status = true };
        }
#endregion

        public void Save()
        {
            _unitOfWork.Save();
        }

        // think this is used by viewplayerscontroller - 
        public OperationStatus UpdateGame(int gameId, int userTeamId, int _teamSize)
        {
            Game _game = null;

            //if (!(_gameRepository.Get(g => g.Id == gameId) == null))
            if (TryGetGame(out _game, gameId))
            {
                IEnumerable<UserTeam_Player> _userTeamPlayers = GetAllChosenUserTeamPlayersForTeam(userTeamId);

                // get number of UserTeams participating in the Game
                int _utCount = _game.UserTeams.Count;

                // if this UserTeam has full allocation of Players & is the last team in the order of play, game = finished
                if ((_userTeamPlayers.Count() == _teamSize) && (_game.CurrentOrderPosition + 1 == _utCount))
                {
                    _game.HasEnded = true;
                    _game.WinnerId = CalculateWinner(gameId);
                    _game.CurrentOrderPosition = -10; //arbitrary integer to ensure it is no userteam's 'go'.
                }
                else
                {
                    int _currentOrderPosition = _game.CurrentOrderPosition;
                    _game.CurrentOrderPosition = ((_currentOrderPosition + 1) == _utCount) ? 0 : _currentOrderPosition + 1;
                }

                OperationStatus _gameSaved = DoUpdateGame(_game);
                if (_gameSaved.Status) return new OperationStatus { Status = true };
            }
            return new OperationStatus { Status = false };
        }

        public OperationStatus SaveUTPlayerAndUpdateGame(int userTeamId, int gameId, int pixelposy, int pixelposx, int playerId)
        {
            var _opStatus = new OperationStatus { Status = true };

            try
            {
                _opStatus = SaveUserTeamPlayer(userTeamId, gameId, 1, 1, playerId);
                _opStatus = UpdateGame(gameId, userTeamId, 1);
                //_unitOfWork.Save();
            }
            catch (Exception exp)
            {
                _opStatus = OperationStatus.CreateFromException("Error saving player and updating game: " + exp.Message, exp);
            }

            return _opStatus;
       }


        ///<summary>
        /// CalculateWinner
        ///</summary>
        ///<remarks>
        /// takes gameid, gets userteams, runs through each userteam, calculates score for each, selects winner, returns id
        /// TODO:
        /// This needs to be moved out into a business/service layer class - it's business logic.
        ///</remarks>
        ///
        public int CalculateWinner(int gameId)
        {
            //topscore = list TopScores
            int _tempScore;
            TopScore _topScore = new TopScore();
            List<UserTeam> _userTeams = GetAllUserTeamsForGame(gameId, "Id");
            List<UserTeam_Player> _players = new List<UserTeam_Player> { };
            Player _player;

            foreach (UserTeam team in _userTeams)
            {
                _players = (List<UserTeam_Player>)GetAllChosenUserTeamPlayersForTeam(team.Id);
                _tempScore = 0;
                foreach (UserTeam_Player player in _players)
                {
                    _player = GetPlayer(player.PlayerId);
                    _tempScore = _tempScore + _player.Age;
                }
                if (_tempScore > _topScore._score)
                {
                    _topScore._userTeamId = team.Id;
                    _topScore._score = _tempScore;
                }
            }
            return _topScore._userTeamId;
        }
    
        protected struct TopScore
        {
            public int _userTeamId { get; set; }
            public int _score { get; set; }
        }

        public void Dispose()
        {
            if (_unitOfWork != null)
                _unitOfWork.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
