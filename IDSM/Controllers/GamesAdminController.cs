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
    public class GamesAdminController : Controller
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

        public GamesAdminController(IService service)
        {
            _service = service;
        }

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ViewResult Index()
        {
            GameViewModel _gvm = new GameViewModel { Games = _service.GetAllGames() };
            return View(_gvm);
        }

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
            IEnumerable<UserProfile> _users = _service.GetAllUsers();
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
            OperationStatus _opStatus = _service.ResetGame(gameId);
            if (_opStatus.Status) _service.Save();
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
            OperationStatus _opStatus = _service.StartGame(gameId);
            if (_opStatus.Status) _service.Save();
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
            OperationStatus _opStatus = _service.AddUserToGame(userId, gameId);
            if (_opStatus.Status) _service.Save();
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
            if (_service.TryGetUserTeam(userTeam: out ut, userTeamId: 0, gameId: gameId, userId: (int)userId))
                return RedirectToAction("Index", "ViewPlayers", new { userTeamId = ut.Id });
            return RedirectToAction("NotFound", "Error");
        }


        #region NOT USED / TO BE IMPLEMENTED
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
        #endregion
    }
}
