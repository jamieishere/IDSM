using IDSM.Model;
using IDSM.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MvcPaging;
using IDSM.Logging.Services.Logging.Log4Net;
using WebMatrix.WebData;
using IDSM.Wrapper;
using IDSM.ViewModel;
using IDSM.Helpers;
using IDSM.Exceptions;
using IDSM.Logging.Services.Logging;

namespace IDSM.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        IGameRepository _gameRepository;
        IUserTeamRepository _userTeamRepository;
        IUserRepository _userRepository;
        IWebSecurityWrapper _wr;

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
        public GameController(IGameRepository gameRepo, IUserTeamRepository userTeamRepo, IWebSecurityWrapper wr, IUserRepository userRepo)
        {
            _gameRepository = gameRepo;
            _userTeamRepository = userTeamRepo;
            _userRepository = userRepo;
            _wr = wr;
        }

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ViewResult Index()
        {
            ILogger _logger = LogFactory.Logger();
            _logger.Debug("testing out whetheritcouldbeantyhing");

            var _games = _gameRepository.GetAllGames();
            GameViewModel _gvm = new GameViewModel { Games = _games };
            return View(_gvm);
        }

        //public ActionResult Details(int id)
        //{
        //    return View(_gameRepository.GetGame(id));
        //}

        /// <summary>
        /// Create
        /// </summary>
        /// <returns>View</returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Create
        /// Model binds posted form values, creates new Game.
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Game game)
        {
            var _opStatus = _gameRepository.CreateGame(game.CreatorId, game.Name);
            ViewBag.OperationStatus = _opStatus;
            return View();
        }

        /// <summary>
        /// ViewUsers
        /// View list of all Users that could be added to the Game.
        /// </summary>
        /// <param name="_game"></param>
        /// <returns>View</returns>
        public ActionResult ViewUsers(Game _game)
        {
            IEnumerable<UserProfile> _users = _userRepository.GetAllUsers();
            AddUserTeamViewModel _vm = new AddUserTeamViewModel() { Users = _users, Game = _game };
            return View(_vm);
        }

        /// <summary>
        /// Deletes all userteams, sets game properties to default
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns>RedirectToAction - Index</returns>
        ///<remarks></remarks>
        public ActionResult ResetGame(int gameId)
        {
            //get all userteams for this game, delete
            IEnumerable<UserTeam> _userTeams = _userTeamRepository.GetAllUserTeamsForGame(gameId, "Id");
            foreach (UserTeam _team in _userTeams)
            {
                //cascading delete setup for UserTeam - UserTeam_Players in IDSMContext
                _userTeamRepository.DeleteUserTeam(_team); 
            }

            //reset all game properties to default
            Game _game = _gameRepository.GetGame(gameId);
            _game.WinnerId = 0;
            _game.HasEnded = false;
            _game.HasStarted = false;
            _game.CurrentOrderPosition = 0;
            _gameRepository.UpdateGame(_game);

            return RedirectToAction("Index");
        }


        /// <summary>
        /// Instantiates game.  Shuffles UserTeams into random order, sets Game properties to 'started'
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns>RedirectToAction - Index</returns>
        /// <remarks></remarks>
        public ActionResult StartGame(int gameId)
        {
            List<UserTeam> _userTeams = _userTeamRepository.GetAllUserTeamsForGame(gameId, "Id");
            _userTeams.Shuffle();
            foreach(UserTeam _team in _userTeams){
                _team.OrderPosition = _userTeams.IndexOf(_team);
                _userTeamRepository.SaveUserTeam(_team);
                //TODO: read what it says here about the command pattern & updating EF entities
                //http://stackoverflow.com/questions/12616276/better-way-to-update-a-record-using-entity-framework
            }

            Game _game = _gameRepository.GetGame(gameId);
            _game.HasStarted = true;
            _gameRepository.UpdateGame(_game);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Adds selected User to this Game.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public ActionResult AddUserToGame(int userId, int gameId)
        {
            // check if userteam exists
            UserTeam _ut = _userTeamRepository.GetUserTeam(userTeamId: 0, gameId: gameId, userId: userId);
            int _intUTID = 0;
            if (_ut == null)
            {
                // no userteam found, create it
                OperationStatus opStatus = _userTeamRepository.CreateUserTeam(userId, gameId);
                if (opStatus.Status) _intUTID = (int)opStatus.OperationID;
            }
            else { _intUTID = _ut.Id; }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Redirect to ViewPlayers for this game/user
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="userId"></param>
        /// <returns>RedirecToAction, or throws an error if can't find the userteam</returns>
        public RedirectToRouteResult ManageUserTeam(int gameId, int? userId)
        {
            // get userteam for this user and this game
            UserTeam ut = _userTeamRepository.GetUserTeam(userTeamId: 0, gameId: gameId, userId: (int)userId);

            if (ut != null)
            {
                  return RedirectToAction("Index", "ViewPlayers", new { userTeamId = ut.Id });
            }
            else {
                Log4NetLogger logger2 = new Log4NetLogger();
                logger2.Error("JoinGame - no userteam found, none created either. userid:" + (int)userId + " gameid:" + gameId);

                //NOTE:
                //Read this. 
                //How to use ActionFilters - handleerrors, outputcache, ValidateAntiForgeryToken, etc
                //http://blogs.msdn.com/b/gduthie/archive/2011/03/17/get-to-know-action-filters-in-asp-net-mvc-3-using-handleerror.aspx

                UserTeamRepositoryException ex = new UserTeamRepositoryException() { BespokeMessage = "Error. An error occurred while processing your request.  Something to do with UserTeams." };
                throw ex;
            }

        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
