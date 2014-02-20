﻿using IDSM.Model;
using System.Web.Mvc;
﻿using IDSM.Repository;
﻿using IDSM.ViewModel;
using IDSM.ServiceLayer;

namespace IDSM.Controllers
{
    [Authorize]
    public class GamesAdminController : Controller
    {
        //IWebSecurityWrapper _wr;
        private readonly IService _service;

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
            ViewBag.OperationStatus = _opStatus;
            return View();
        }

        public ViewResult ViewUsers(Game game)
        {
            return View(_service.GetAddUserTeamViewModelForGame(game));
        }

        /// <summary>
        /// Deletes all userteams, sets game properties to default
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns>RedirectToAction - Index</returns>
        ///<remarks></remarks>
        public ActionResult ResetGame(int gameId)
        {
            _service.ResetGame(gameId);
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
            _service.StartGame(gameId);
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
            _service.AddUserToGame(userId, gameId);
            return RedirectToAction("Index");
        }
    }
}
