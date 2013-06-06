using IDSM.Model;
using IDSM.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using IDSM.Logging.Log4Net;
using MvcPaging;

namespace IDSM.Controllers
{
    public class GameController : Controller
    {
        IGameRepository _GameRepository;
        IUserTeamRepository _UserTeamRepository;

        public GameController(IGameRepository gameRepo, IUserTeamRepository userRepo)
        {
            // don't need this now thanks to UNITY?  or dependency injection?  are they the same thing?
            //_UserRepository = userRepo ?? ModelContainer.Instance.Resolve<UserRepository>();
            //_PlayerRepository = playerRepo ?? ModelContainer.Instance.Resolve<PlayerRepository>();
            _GameRepository = gameRepo;
            _UserTeamRepository = userRepo;
        }

        //
        // GET: /Game/

        public ActionResult Index()
        {
             ViewBag.ErrorMessage = "this is the error message";
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
        public ActionResult JoinGame(int gameid)
        {
            // get current userid & along with game id, create a userteam

            //Get UserID  WebMatrix.WebData.WebSecurity.CurrentUserId
            //Old way - Guid userGuid = (Guid)Membership.GetUser().ProviderUserKey;  
            //also, set a cache value so don't have to hit the Db everytime Cache.Add(User.Identity.Name, user.UserID); // Key: Username; Value: Guid.

            // ok need to add a 'getuser' method to userrepository first ... nah. i just need th id ...
            // ok need to think this thru a bit.... all i really need here is the id o fhte user & the game...
            // its overkill faffing getting users... may need to refactor.. its cos i use the datacontext method of saving - where i need to use an
            // instance of the actual userteam... which reqquires an instance of the user....  nah actually that is ok.. i think.. doesnt require DB access, just requires creation of 'dummy' instances..
            // is that good practice????

            // create dummy user instance with the ID i have.  only have the GUID.  need to get the ID from the GUID.  so need a new method in teh userrepo for this...
            // is there a better way?

            int UserID = WebMatrix.WebData.WebSecurity.CurrentUserId;
          //  Guid userGuid = (Guid)Membership.GetUser().ProviderUserKey;

            // was just heppenin that no id was on the querystring
            // guessing this was that the operationstatus status was false ..
 
            // want the check if a record exists in the userteam table with current user id & game id
            // if it doesnt, create it
            // return the userteamID
            // this is business logic really, so i want to pull out of the controller?

            

            Log4NetLogger logger2 = new Log4NetLogger();
            logger2.Info("Test message for Log4Net");

            

            //nned to figure out how i want to do this - where should the logic go - i moved itno here as i thoguth bestt to keep create & get separeate
            // but now it complicates the logic in here
            UserTeam ut = _UserTeamRepository.GetUserTeam(userteamid:0, gameid: gameid, userid: UserID);
            int intUTID = 0;
            if (ut == null)
            {
                OperationStatus opStatus = _UserTeamRepository.CreateUserTeam(UserID, gameid);
                if (opStatus.Status) intUTID = (int)opStatus.OperationID;
            }
            else {intUTID = ut.Id; }
            

            // need logging .

            if (intUTID > 0)
            {
                return RedirectToAction("Index", "ViewPlayers", new { id = intUTID });
            }
            else
            {
                // do some logging. actually, no need - logging will be done within OperationStatus
                ViewBag.ErrorMessage = "this is the error message";//opStatus.ExceptionMessage;
                // do need a better way to handle the error than  just returning to the same page & not saying what happened.
                // so do want to display the message on screen really
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
