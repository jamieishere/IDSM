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

namespace IDSM.Controllers
{
            [Authorize]
    public class GameController : Controller
    {
        IGameRepository _GameRepository;
        IUserTeamRepository _UserTeamRepository;
        IWebSecurityWrapper _wr;

        public GameController(IGameRepository gameRepo, IUserTeamRepository userRepo, IWebSecurityWrapper wr)
        {
            // Using dependency injection with Unity
            // we don't need to resolve the model isntances here anymore - unity handles this.
            //_UserRepository = userRepo ?? ModelContainer.Instance.Resolve<UserRepository>();
            //_PlayerRepository = playerRepo ?? ModelContainer.Instance.Resolve<PlayerRepository>();
            _GameRepository = gameRepo;
            _UserTeamRepository = userRepo;
            _wr = wr;
        }

        //
        // GET: /Game/

        public ViewResult Index()
        {
           //  ViewBag.ErrorMessage = "this is the error message";
            var games = _GameRepository.GetAllGames();
            return View(games);
        }

        //
        // GET: /Game/Details/5

        public ActionResult Details(int id)
        {
            return View(_GameRepository.GetGame(id));
        }

        public ActionResult Create()
        {
            return View();
        }


        //
        // Post: /Game/Create
        [HttpPost]
        public ActionResult Create(Game game)
        {
            var opStatus = _GameRepository.SaveGame(game.CreatorId, game.Name);

            return View("Index", _GameRepository.GetAllGames());
        }

        // Get: /Game/JoinGame
       // public RedirectToRouteResult JoinGame(int gameid, IWebSecurityWrapper wr)
        public RedirectToRouteResult JoinGame(int gameid)
        {
            
            //Old way - Guid userGuid = (Guid)Membership.GetUser().ProviderUserKey;  
            //also, set a cache value so don't have to hit the Db everytime Cache.Add(User.Identity.Name, user.UserID); // Key: Username; Value: Guid.
           int UserID =  _wr.CurrentUserId; //WebSecurity.CurrentUserId;      
            //Membership.GetUser().UserName

            // get userteam for this user and this game
            UserTeam ut = _UserTeamRepository.GetUserTeam(userteamid:0, gameid: gameid, userid: UserID);
            int intUTID = 0;
            if (ut == null)
            {
                // no userteam found, create it
                OperationStatus opStatus = _UserTeamRepository.CreateUserTeam(UserID, gameid);
                if (opStatus.Status) intUTID = (int)opStatus.OperationID;
            }
            else {intUTID = ut.Id; }
            //intUTID = 0;
            if (intUTID > 0)
            {
                return RedirectToAction("Index", "ViewPlayers", new { id = intUTID });
            }
            else
            {
                Log4NetLogger logger2 = new Log4NetLogger();
                logger2.Error("JoinGame - no userteam found, none created either. userid:"+UserID+" gameid:"+gameid);

                // can't set a viewbag value as MVC is stateless.  must pass an error message to the action.
                // need to set route values.
                TempData["err"] = "There was an error.  The administrator has been informed.";

                return RedirectToAction("Index", "Game");
            }
        }

        //
        // POST: /Game/Create

        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //
        // GET: /Game/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Game/Edit/5

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

        //
        // GET: /Game/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Game/Delete/5

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
