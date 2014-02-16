using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDSM.Model;
using IDSM.Repository;
using System.Transactions;
using System.Data.Common;

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

        public IUserRepository Users { get { return _userRepository; } }
        public IUserTeamRepository UserTeams { get { return _userTeamRepository; } }
        public IPlayerRepository Players { get { return _playerRepository; } }
        public IGameRepository Games { get { return _gameRepository; } }
        public IUserTeam_PlayerRepository UserTeamPlayers { get { return _userTeamPlayerRepository; } }

        #region GAMES METHODS


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

        /// GetAllGames
        /// Gets all Games
        /// </summary>
        /// <returns>IEnumerable<Game></returns>
        public IEnumerable<Game> GetAllGames()
        {
            var _games = _gameRepository.GetAllGames();
            return _games;
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

        public OperationStatus CreateGame(int creatorId, string name)
        {
            Game game = new Game() { CreatorId = creatorId, Name = name };
            OperationStatus _opStatus = _gameRepository.Create(game);
            return _opStatus;
        }

        #endregion

        public void Save()
        {
            _unitOfWork.Save();
        }

        public OperationStatus UpdateGame(int gameId, int userTeamId, int _teamSize)
        {
            Game _game = null;

            //if (!(_gameRepository.Get(g => g.Id == gameId) == null))
            if (TryGetGame(out _game, gameId))
            {
                IEnumerable<UserTeam_Player> _userTeamPlayers = _playerRepository.GetAllChosenPlayersForUserTeam(userTeamId);

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

                OperationStatus _gameSaved = _gameRepository.DoUpdateGame(_game);
                if (_gameSaved.Status) return new OperationStatus { Status = true };
            }
            return new OperationStatus { Status = false };
        }

        public OperationStatus SaveUTPlayerAndUpdateGame(int userTeamId, int gameId, int pixelposy, int pixelposx, int playerId)
        {
            var _opStatus = new OperationStatus { Status = true };

           // using (TransactionScope transaction = new TransactionScope())
         //   {
                try
                {
                    _opStatus = SaveUTPlayer(userTeamId, gameId, 1, 1, playerId);
                    _opStatus = UpdateGame(gameId, userTeamId, 1);
                    _unitOfWork.Save();
                }
                catch (Exception exp)
                {
                    _opStatus = OperationStatus.CreateFromException("Error saving player and updating game: " + exp.Message, exp);
                }

               // if (_opStatus.Status) transaction.Complete();
                return _opStatus;
           // }
       } 

        public OperationStatus SaveUTPlayer(int userteamid, int gameid, int pixelposy, int pixelposx, int playerid)
        {
            UserTeam_Player player = new UserTeam_Player() { UserTeamId = userteamid, GameId = gameid, PixelPosX = pixelposx, PixelPosY = pixelposy, PlayerId = playerid };

            var temp = _userTeamPlayerRepository.Get(p => p.PlayerId == playerid && p.UserTeamId == userteamid);
            var temp2 = _gameRepository.Get(g => g.Id == 1);

            // if userteam_player already exists, return true
            if (temp != null) return new OperationStatus { Status = true };

            try
            {
                _unitOfWork.Context.UserTeam_Players.Add(player);
            }
            catch (Exception ex)
            {
                return OperationStatus.CreateFromException("Error - SaveUserTeamPlayer.", ex);
            }
            return new OperationStatus { Status = true };
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
            List<UserTeam> _userTeams = _userTeamRepository.GetAllUserTeamsForGame(gameId, "Id");
            List<UserTeam_Player> _players = new List<UserTeam_Player> { };
            Player _player;

            foreach (UserTeam team in _userTeams)
            {
                _players = (List<UserTeam_Player>)_playerRepository.nGetAllChosenPlayersForUserTeam(team.Id);
                _tempScore = 0;
                foreach (UserTeam_Player player in _players)
                {
                    _player = _playerRepository.GetPlayer(player.PlayerId);
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
