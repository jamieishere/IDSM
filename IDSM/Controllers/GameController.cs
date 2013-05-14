using IDSM.Model;
using IDSM.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

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

            //check if a record exists in the userteam table with current user id.
            // if not, create the team
            // then redirect/

            var opStatus = _UserTeamRepository.CreateUserTeam(UserID, gameid);

            // NAH the create method has to return the newly created USERTEAMID... maybe need to update the operationstatus object, or utilise it better in the create method-populated the ID field.
            // instead of that - just redirect to the viewplayers.  in that index action, it gets the currentuser, finds the record for userteam based on that
            // better anyway.... still though - do need to find out how to return an id from datacontext.save.



            //if (opStatus.Status)
            //{

            return View("ViewPlayers");
            //}
            //else
            //{
            //    return View("Index", _GameRepository.GetAllGames());
            //}
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
