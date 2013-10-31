using IDSM.Model;
using IDSM.Models;
using IDSM.Repository;
using IDSM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using System.Web.Security;
using System.Diagnostics;
using IDSM.Logging.Services.Logging.Log4Net;


namespace IDSM.Controllers
{
    public class ViewPlayersController : Controller
    {
        IUserRepository _userRepository;
        IUserTeamRepository _userTeamRepository;
        IPlayerRepository _playerRepository;
        IGameRepository _gameRepository;

        private const int _numPlayersInTeam = 1;

        /// <summary>
        /// Constructor using dependency injection with Unity
        /// </summary>
        /// <param name="gameRepo"></param>
        /// <param name="userTeamRepo"></param>
        /// <param name="wr"></param>
        /// <param name="userRepo"></param>
        /// <remarks>
        /// Model instances resolved in IDSM.Model.ModelContainer if not passed explicitly
        /// Allowing them to be passed into constructor is basis of Unit Testing.
        /// </remarks>
        public ViewPlayersController(IUserRepository userRepo, IPlayerRepository playerRepo, IGameRepository gameRepo, IUserTeamRepository userTeamRepo)
        {
            _userRepository = userRepo;
            _playerRepository = playerRepo;
            _gameRepository = gameRepo;
            _userTeamRepository = userTeamRepo;
        }

        /// <summary>
        /// Return View displaying current ChosenPlayers for the current UserTeam, plus the filtered list (depending on selected club/searchstring) of Players.
        /// </summary>
        /// <param name="userTeamId"></param>
        /// <param name="footballClub"></param>
        /// <param name="searchString"></param>
        /// <returns>Index View</returns>
        /// <remarks>
        /// TODO:
        /// Caching.
        /// </remarks>
        public ActionResult Index(int userTeamId, string footballClub, string searchString)
        {

            Log4NetLogger logger2 = new Log4NetLogger();
            logger2.Error("ViewPlayers");
            logger2.Info("ViewPlayers");

            //TODO:
            //Understand difference between creating variable as IList v List v IEnumerable
            // using ienumerable means the linq compiler has chance to optimise.  list doesnt.
            // so really, i want to return an ienumerable whenever I just plan on iterating over a collection (not adding/subtracting)
            // Read these:
            // http://stackoverflow.com/questions/3628425/ienumerable-vs-list-what-to-use-how-do-they-work
            // http://stackoverflow.com/questions/4180767/whats-the-difference-between-liststring-and-ienumerablestring
            // http://stackoverflow.com/questions/2984045/linq-c-where-foreach-using-index-in-a-list-array
            // http://stackoverflow.com/questions/1924535/c-any-benefit-of-listt-foreach-over-plain-foreach-loop
            // http://stackoverflow.com/questions/183791/how-would-you-do-a-not-in-query-with-linq
            // Question:
            // if ienumerable is impemented by a list, why can't it handle it?  why do i have to call tolist() / cast to list?

            // Understand the disposed tracker
            // How does the context get disposed everytime? 
            // I use the same one for all repositorys.. i need to get rid then use again creating a new... if its disposed, then i wanna create new
            
            ViewBag.FootballClub = new SelectList(_playerRepository.GetAllClubs());
            IEnumerable<Player> _footballPlayers = _playerRepository.GetAllPlayers();
            UserTeam _userTeam = _userTeamRepository.GetUserTeam(userTeamId,0,0);
            UserProfile _user = _userRepository.GetUser(_userTeam.UserId);
            int[] _chosenPlayerIds = _playerRepository.GetAllChosenPlayerIdsForGame(_userTeam.GameId);
            IEnumerable<UserTeam_Player> _chosenPlayers = _playerRepository.GetAllChosenPlayersForUserTeam(_userTeam.Id);
            Game _game = _gameRepository.GetGame(_userTeam.GameId);

            //update player list, marking those already chosen
            foreach (Player p in _footballPlayers){
                if (_chosenPlayerIds.Contains(p.Id)){p.HasBeenChosen = true;}
            }

            //filter players by searchstring
            if (!String.IsNullOrEmpty(searchString))
            {
                _footballPlayers = _footballPlayers.Where(s => s.Name.Contains(searchString));
            }

            //filter players by club
            if (!String.IsNullOrEmpty(footballClub))
            {
                _footballPlayers = _footballPlayers.Where(x => x.Club == footballClub);
            }

            // setup message to be displayed to User once they have added their chosen player
            string _addedPlayerMessage = "Current player is {0}.  There are {1} turns left before your go.";            
            string _tmpActiveUserName;
            int _tmpTurnsLeft = 0;

            if (_game.HasEnded){ _addedPlayerMessage = "The game has ended.";}
            else
            {
                // figure out which is the currently 'active' userteam
                // this is overly complex, might be better to store active team id against the game.
                // also might be better to store the 'round' against the game.
                // these values are queried/required a fair bit... it is redundant tho..

                // get all userteams - this is bandwidth overkill, I only need the count.
                List<UserTeam> _userTeamsForGame = _userTeamRepository.GetAllUserTeamsForGame(_game.Id, "Id");
               


                if (_userTeam.OrderPosition != _game.CurrentOrderPosition)
                {
                    UserTeam _activeUt = _userTeamRepository.GetUserTeamByOrderPosition(_game.CurrentOrderPosition, _game.Id);
                    // User doesn't exist now for UserTeam
                    // Have to write new method to get a User from the Id
                    //_tmpActiveUserName = _activeUt.User.UserName;
                    _tmpActiveUserName = _userRepository.GetUser(_activeUt.UserId).UserName;

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

            return View(new SearchViewModel() { Players_SearchedFor = _footballPlayers, Players_Chosen = _chosenPlayers, GameId = _userTeam.GameId, GameName = _game.Name, GameCurrentOrderPosition = _game.CurrentOrderPosition, UserTeamId = _userTeam.Id, UserName = _user.UserName, UserTeamOrderPosition = _userTeam.OrderPosition, AddedPlayerMessage = _addedPlayerMessage });
        }

        /// <summary>
        /// Adds the selected player to the UserTeam
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userteamid"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public ActionResult AddPlayer(int id, int userteamid, int gameId)
        {
            var _pr = new PlayerRepository();
            Player _player = _playerRepository.GetPlayer(id);

            //create a userteam player using this player
            UserTeam_Player _utPlayer = new UserTeam_Player();
            _utPlayer.PlayerId = id;
            //_utPlayer.Player = _player;

            //    //***********************************************************
            //    // TODO:
            //    // Prevent duplicates being created.
            //    var ut = new UserTeamRepository();
            //    ut.SaveUserTeamPlayer(userteamid, gameId, 1, 1, id);
            //    //***********************************************************

            // save the newly created userteam player to the userteam
            var _ut = new UserTeamRepository();
            OperationStatus _op = _ut.SaveUserTeamPlayer(userteamid, gameId, 1, 1, id);

            // figure out if this is the end of the game / update
            Game game = _gameRepository.GetGame(gameId);
            List<UserTeam> _userTeams = _userTeamRepository.GetAllUserTeamsForGame(gameId, "Id");
            int _utCount = _userTeams.Count;
            List<UserTeam_Player> _userTeamPlayers = (List<UserTeam_Player>)_playerRepository.GetAllChosenPlayersForUserTeam(userteamid);

            // if this userteam has full allocation of players & is the last team in the order of play, game finished
            if ((_userTeamPlayers.Count == _numPlayersInTeam) && (game.CurrentOrderPosition+1 == _utCount))
            {
                game.HasEnded = true;
                game.WinnerId = CalculateWinner(gameId);
                game.CurrentOrderPosition = -10; //arbitrary integer to ensure it is no userteam's 'go'.
            }
            else
            {
                int _currentOrderPosition = game.CurrentOrderPosition;
                game.CurrentOrderPosition = ((_currentOrderPosition + 1) == _userTeams.Count) ? 0 : _currentOrderPosition + 1;
            }

            _gameRepository.UpdateGame(game);

            return RedirectToAction("Index", new { userteamid = userteamid });
        }

        ///<summary>
        /// CalculateWinner
        ///</summary>
        ///<remarks>
        /// takes gameid, gets userteams, runs through each userteam, calculates score for each, selects winner, returns id
        /// TODO:
        /// This needs to be moved out into a business/service layer class - it's business logic.
        ///</remarks>
        private int CalculateWinner(int gameId)
        {
            //topscore = list TopScores
            int _tempScore;
            TopScore _topScore = new TopScore();
            List<UserTeam> _userTeams = _userTeamRepository.GetAllUserTeamsForGame(gameId,"Id");
            List<UserTeam_Player> _players = new List<UserTeam_Player> { };
            Player _player;

            foreach (UserTeam team in _userTeams)
            {
                _players = (List<UserTeam_Player>)_playerRepository.GetAllChosenPlayersForUserTeam(team.Id);
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
            public int _userTeamId{get;set;}
            public int _score { get; set; }
        }


        ///<summary>
        /// ViewPlayers Index action
        ///<remarks>
        /// Unused action - previously used in conjunction with BlankPlayerRow & ChosenTeamViewModel - when user submitted player choises, the posted form values would be auto bound to the ChosenTeamViewModel and saved to the database.
        ///</remarks>
        //[HttpPost]
        //public ActionResult Index(ChosenTeamViewModel ct)
        //{
        //    var OpStatus = new OperationStatus() { Status = false };
        //    ViewBag.FootballClub = new SelectList(_playerRepository.GetAllClubs());

        //    Debug.Assert(ct.UserTeam_Players != null);
        //    OpStatus = _userTeamRepository.SaveUserTeam(ct.UserTeamID, ct.GameID, ct.UserTeam_Players);

        //    if (!OpStatus.Status)
        //    {
        //        ViewBag.OperationStatus = OpStatus;
        //    }

        //    return RedirectToAction("Index", "ViewPlayers", new { userTeamId = ct.UserTeamID });
        //}

        /////<summary>
        ///// ViewResult BlankPlayerRow
        /////<remarks>
        ///// Unused.  Utilised HTMLPrefix Extension method BeginCollectionItem to create a dynamic list.
        /////</remarks>
        //public PartialViewResult BlankPlayerRow(int id, int userteamid, int gameId)
        //{
        //    var pr = new PlayerRepository();
        //    Player player = _playerRepository.GetPlayer(id);
        
        //    //create a userteam player using this player
        //    UserTeam_Player utplayer = new UserTeam_Player();
        //    utplayer.PlayerId = id;
        //    utplayer.Player = player;

        //    //***********************************************************
        //    // TODO:
        //    // Prevent duplicates being created.
        //    var ut = new UserTeamRepository();
        //    ut.SaveUserTeamPlayer(userteamid, gameId, 1, 1, id);
        //    //***********************************************************

        //    //update game currentorderposition
        //    Game game = _gameRepository.GetGame(gameId);
        //    List<UserTeam> uts = _userTeamRepository.GetAllUserTeamsForGame(gameId, "Id");
        //    int _utCount = uts.Count;
        //    int _currentOrderPosition = game.CurrentOrderPosition;
        //    game.CurrentOrderPosition = ((_currentOrderPosition + 1) == uts.Count) ? 0 : _currentOrderPosition + 1;

        //    List<UserTeam_Player> utps = (List<UserTeam_Player>)_playerRepository.GetAllChosenPlayersForUserTeam(userteamid);

        //    if ((utps.Count == _numPlayersInTeam) && (game.CurrentOrderPosition == _utCount - 1))
        //    {
        //        game.HasEnded = true;
        //        game.WinnerId = CalculateWinner(gameId);
        //    }

        //    _gameRepository.UpdateGame(game);
        //    return PartialView("ChosenPlayerRow", utplayer);
        //}

    }
}
