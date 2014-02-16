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
using AutoMapper;
using IDSM.Logging.Services.Logging;
using System.Transactions;
using System.Text;
using IDSM.ServiceLayer;

namespace IDSM.Controllers
{
    public class ViewPlayersController : Controller
    {
        //IUserRepository _userRepository;
        //IUserTeamRepository _userTeamRepository;
        //IPlayerRepository _playerRepository;
        //IGameRepository _gameRepository;
        //private UnitOfWork unitOfWork = new UnitOfWork();


        private IService _service;


        private const int _teamSize = 1;

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
        //public ViewPlayersController(IUserRepository userRepo, IPlayerRepository playerRepo, IGameRepository gameRepo, IUserTeamRepository userTeamRepo)
        //{
        //    _userRepository = userRepo;
        //    _playerRepository = playerRepo;
        //    _gameRepository = gameRepo;
        //    _userTeamRepository = userTeamRepo;
        //}
        public ViewPlayersController(IService service)
        {
            _service = service;
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
            IEnumerable<string> _clubs = null;
            IEnumerable<Player> _footballPlayers = null;
            UserTeam _userTeam = null;
            UserProfile _user = null;
            int[] _chosenPlayerIds = null;
            IEnumerable<UserTeam_Player> _chosenPlayers = null;
            Game _game = null;

            // get all clubs and players
            _clubs = _service.Players.GetAllClubs(); // if null, this should throw a custom exception
             _footballPlayers = _service.Players.GetAllPlayers(); // if null, this should throw a cusom exception.
            ViewBag.FootballClub = new SelectList(_clubs);

            // get this UserTeam, User and Game
            if (!_service.UserTeams.TryGetUserTeam(out _userTeam, userTeamId: userTeamId))
                return RedirectToAction("ApplicationError", "Error");
            if (!_service.Users.TryGetUser(out _user, _userTeam.UserId))
                return RedirectToAction("ApplicationError", "Error");
            if (!_service.TryGetGame(out _game, _userTeam.GameId))
                return RedirectToAction("ApplicationError", "Error");

            // setup chosenplayers for the game, and for this team
            _chosenPlayerIds = _service.Players.GetAllChosenPlayerIdsForGame(_userTeam.GameId);
            _chosenPlayers = _service.Players.GetAllChosenPlayersForUserTeam(_userTeam.Id);
                
            // map Player to DTO object (has extra properties)
            IEnumerable<PlayerDto> _footballPlayersDto = new List<PlayerDto>();
            Mapper.CreateMap<Player, PlayerDto>();
            _footballPlayersDto = Mapper.Map<IEnumerable<PlayerDto>>(_footballPlayers);

            //update player list, marking those already chosen
            foreach (PlayerDto p in _footballPlayersDto)
            {
                if (_chosenPlayerIds.Contains(p.Id)){p.HasBeenChosen = true;}
            }

            //if passed, filter players by searchstring
            if (!String.IsNullOrEmpty(searchString))
            {
                _footballPlayersDto = _footballPlayersDto.Where(s => s.Name.Contains(searchString));
            }

            //if passed, filter players by club
            if (!String.IsNullOrEmpty(footballClub))
            {
                _footballPlayersDto = _footballPlayersDto.Where(x => x.Club == footballClub);
            }

            // setup message to be displayed to User once they have added their chosen player
            string _addedPlayerMessage = "Current player is {0}.  There are {1} turns left before your go.";            
            string _tmpActiveUserName;
            int _tmpTurnsLeft = 0;

            //TODO:
            // refactor this - if the game has ended, this entire controller action should be skipped.
            if (_game.HasEnded){ _addedPlayerMessage = "The game has ended.";}
            else
            {
                //List<UserTeam> _userTeamsForGame = _userTeamRepository.GetAllUserTeamsForGame(_game.Id, "Id");
                List<UserTeam> _userTeamsForGame = _service.UserTeams.GetAllUserTeamsForGame(_game.Id, "Id");
               
                if (_userTeam.OrderPosition != _game.CurrentOrderPosition)
                {
                   // UserTeam _activeUt = _userTeamRepository.GetUserTeamByOrderPosition(_game.CurrentOrderPosition, _game.Id);
                    UserTeam _activeUt = _service.UserTeams.GetUserTeamByOrderPosition(_game.CurrentOrderPosition, _game.Id);
                    // User doesn't exist now for UserTeam
                    // Have to write new method to get a User from the Id
                    //_tmpActiveUserName = _activeUt.User.UserName;
                   // _tmpActiveUserName = _userRepository.GetUser(_activeUt.UserId).UserName;
                    _tmpActiveUserName = _service.Users.Get(u => u.UserId == _activeUt.UserId).UserName;

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

            return View(new SearchViewModel() { Players_SearchedFor = _footballPlayersDto, Players_Chosen = _chosenPlayers, GameId = _userTeam.GameId, GameName = _game.Name, GameCurrentOrderPosition = _game.CurrentOrderPosition, UserTeamId = _userTeam.Id, UserName = _user.UserName, UserTeamOrderPosition = _userTeam.OrderPosition, AddedPlayerMessage = _addedPlayerMessage });
        }

        /// <summary>
        /// Adds the selected player to the UserTeam
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="userTeamId"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public ActionResult AddPlayer(int playerId, int userTeamId, int gameId)
        {
            if(AddUserTeamPlayer(playerId, userTeamId, gameId))
            {
                ViewBag.Status = "Player added";
            }
            else
            { 
                ViewBag.Status = "Thre was a problem, player not added"; 
            }

            return RedirectToAction("Index", new { userteamid = userTeamId });
        }

        private bool AddUserTeamPlayer(int playerId, int userTeamId, int gameId)
        {
            Player _player = null;

            // check a Player exists for this playerId
            if (!_service.Players.TryGetPlayer(out _player, playerId)) return false;

            try
            {
                    // save the UserTeam_Player
               // OperationStatus _op = _userTeamRepository.SaveUTPlayerAndUpdateGame(userTeamId, gameId, 1, 1, playerId);
                OperationStatus _op = _service.SaveUTPlayerAndUpdateGame(userTeamId, gameId, 1, 1, playerId);

            }
            catch (Exception ex)
            {
                ILogger _logger = LogFactory.Logger();
                _logger.Error(String.Format("AddUserTeamPlayer failed. playerId:{0}, userTeamId:{1}, gameId:{2}", playerId, userTeamId, gameId), ex);
                return false;
            }
            return true;
        }
    }
}
