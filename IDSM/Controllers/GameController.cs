﻿using IDSM.Model;
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
using AutoMapper;
using IDSM.ServiceLayer;

namespace IDSM.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        //IWebSecurityWrapper _wr;
        private IService _service;

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
        //public GameController(IGameRepository gameRepo, IUserTeamRepository userTeamRepo, IWebSecurityWrapper wr, IUserRepository userRepo)
        //{
        //    _gameRepository = gameRepo;
        //    _userTeamRepository = userTeamRepo;
        //    _userRepository = userRepo;
        //    _wr = wr;
        //}

        public GameController(IService service)
        {
            _service = service;
        }


        public UserProfile GetUser(UserTeam ut)
        {
            UserProfile _user = _service.Users.Get(u => u.UserId == ut.UserId);
            return _user;
        }

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ViewResult Index()
        {
            ILogger _logger = LogFactory.Logger();
            _logger.Debug("in index");

            //var _games = _service.Games.GetAllGames();
            var _games = _service.GetAllGames();

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
        public ViewResult Create()
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
        public ViewResult Create(Game game)
        {
            OperationStatus _opStatus = _service.CreateGame(game.CreatorId, game.Name);
            if (_opStatus.Status) _service.Save();
            ViewBag.OperationStatus = _opStatus;
            return View();
        }

        /// <summary>
        /// ViewUsers
        /// View list of all Users that could be added to the Game.
        /// </summary>
        /// <param name="_game"></param>
        /// <returns>View</returns>
        public ViewResult ViewUsers(Game _game)
        {
            IEnumerable<UserProfile> _users = _service.Users.GetAllUsers();
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
            IEnumerable<UserTeam> _userTeams = _service.UserTeams.GetAllUserTeamsForGame(gameId, "Id");
            if (_userTeams != null)
            {
                foreach (UserTeam _team in _userTeams)
                {
                    //cascading delete setup for UserTeam - UserTeam_Players in IDSMContext
                    _service.UserTeams.DeleteUserTeam(_team);
                }

                //reset all game properties to default
                Game _game = _service.GetGame(gameId);
                _game.WinnerId = 0;
                _game.HasEnded = false;
                _game.HasStarted = false;
                _game.CurrentOrderPosition = 0;
                _service.Games.DoUpdateGame(_game);
                _service.Save();
            }
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
            List<UserTeam> _userTeams = _service.UserTeams.GetAllUserTeamsForGame(gameId, "Id");
            if (_userTeams != null)
            {
                _userTeams.Shuffle();
                foreach (UserTeam _team in _userTeams)
                {
                    _team.OrderPosition = _userTeams.IndexOf(_team);
                    _service.UserTeams.SaveUserTeam(_team);
                    //_service.UserTeams.Save(_team);
                    //TODO: read what it says here about the command pattern & updating EF entities
                    //http://stackoverflow.com/questions/12616276/better-way-to-update-a-record-using-entity-framework
                }

                Game _game = _service.GetGame(gameId);
                _game.HasStarted = true;
                _service.Games.DoUpdateGame(_game);
                _service.Save();
            }
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
            UserTeam ut = null;

            if (!_service.UserTeams.TryGetUserTeam(userTeamId: 0, gameId: gameId, userId: (int)userId, userTeam: out ut))
            {
                try
                {
                    OperationStatus opStatus = _service.UserTeams.CreateUserTeam(userId, gameId);
                    _service.Save();
                }
                catch
                {
                    ViewBag.Message = "Error saving team.";
                }
            }

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
            UserTeam ut = null;

           // if (_userTeamRepository.TryGetUserTeam(userTeamId: 0, gameId: gameId, userId: (int)userId, userTeam :  out ut))
            if (_service.UserTeams.TryGetUserTeam(userTeamId: 0, gameId: gameId, userId: (int)userId, userTeam: out ut))
            
            {
                return RedirectToAction("Index", "ViewPlayers", new { userTeamId = ut.Id });
            }

            return RedirectToAction("NotFound", "Error");
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
