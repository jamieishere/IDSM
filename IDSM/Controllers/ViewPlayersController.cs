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


namespace IDSM.Controllers
{
    public class ViewPlayersController : Controller
    {
        //IAccountRepository _AccountRepository;
        //ISecurityRepository _SecurityRepository;
        //IMarketsAndNewsRepository _MarketRepository;
        IUserTeamRepository _UserRepository;
        IPlayerRepository _PlayerRepository;


        ///<summary>
        /// ViewPlayers constructor
        ///</summary>
        ///<remarks>
        /// Parameter types are interfaces - means the actual repository is NOT hardcoded, thus we can pass in different instances for testing etc
        /// As a fall back (if the parameters are null, ie called the constructor without passing anything), we COULD hard code our default repositories
        ///     Instead we do dependency injection using Microsoft Unity
        ///</remarks>
        public ViewPlayersController(IUserTeamRepository userRepo, IPlayerRepository playerRepo)
        {
            //_UserRepository = userRepo ?? ModelContainer.Instance.Resolve<UserRepository>();
            //_PlayerRepository = playerRepo ?? ModelContainer.Instance.Resolve<PlayerRepository>();
            _UserRepository = userRepo;
            _PlayerRepository = playerRepo;
        }

        [HttpPost]
        //public ActionResult Index(IEnumerable<Player> players)
        public ActionResult Index(ChosenTeamViewModel ct)
        {
            //var ur = new UserRepository();
            //var pr = new PlayerRepository();
            var opStatus = new OperationStatus() { Status = false };
            ViewBag.footballClub = new SelectList(_PlayerRepository.GetAllClubs());

            //Get UserID  WebMatrix.WebData.WebSecurity.CurrentUserId
            //Old way - Guid userGuid = (Guid)Membership.GetUser().ProviderUserKey;  
            // also, set a cache value so don't have to hit the Db everytime Cache.Add(User.Identity.Name, user.UserID); // Key: Username; Value: Guid.

            //i actually I don't need userID here, I need to return a viewmodel  so i can have the players and the userteam id...  not sure how to populate from the view tho
            //userteam could be in the viewbag?  no, viewmodel is probaly better...
 
            // BUT... how do i create the viewmodel in BeginFormCollection?  Perhaps I just create a viewmodel that has players, userid, gameid as a poperty and then add those as hidden fields in the form?
            //  even if this works, i still don't get how the form elements that BeginFormCollection lays down are needed. Why the first one players.index???

            // OK THIS WORKED.  MVC basically figures out from the formcollection how to bind to a viewmodel.  nice.  however still dont get why that players.index is needed.  what happens to it on binging?  does it just get discarded?
                
            opStatus = _UserRepository.SaveUserTeam(ct.GameID, ct.UserTeamID, ct.Players);

            if (!opStatus.Status)
            {
                ViewBag.OperationStatus = opStatus;
            }
            return View(new SearchViewModel() { Players_SearchedFor = _PlayerRepository.GetAllPlayers(), Players_Chosen = new List<Player>() });
        }

        public ActionResult Index(string footballClub, string searchString)
        {
            //var pr = new PlayerRepository();

            //probably needs refactoring to handle errors better in the get methods, also i need to undertsand IList/List
            // for example - its' better to use IList - why exactly
            // when i changed footballplayers to IList (either in repository or in here), this footballPlayers.Where(s => s.Name.Contains(searchString));  
            //    doesnt work - says cannot convert inermuable to ilist
            //    could only get it to work if GetAllPlayers(); returned an IEnumerable<Player> rather than IList<Player> or List<Player>
            //    WHY IS THIS?  WHAT ARE THE DIFFERENCES
            //   http://stackoverflow.com/questions/4180767/whats-the-difference-between-liststring-and-ienumerablestring
            //  except that seems to say list is a more comprehensive expression... list implements ienumerable...
            //  so why can't i do s => s.Name.Contains(searchString)  on a list but i can on an inerumber?

            // I can if I add .ToList(); to the end of the expression.
            // WHAT?
            //  I knwo what to DO but I don't understand it.
            // i guess it's because the linq statment returns an ienumerable, not a list.  my question really is thoug, if ienumerable is impemented by a list, why can't it handle it?  why do i have to call tolist()?
            // ok it's down to optimisation
            // using ienumerable means the linq compiler has chance to optimise.  list doesnt.
            // so really, i want to return an ienumerable because the players isn't going to have anything added to it (which a list enables)
            // read this http://stackoverflow.com/questions/3628425/ienumerable-vs-list-what-to-use-how-do-they-work

            // also need to undersatsnd the disposed tracker... - how does the context get dispaosed everytime, and.. ah i get it. i use the same one for all repositorys.. i need to get rid then use again creating a new... if its disposed, then i wanna create new
            // still need to undsertand the code tho

            // ok now.. got no players in my model

            //need to understand mocking and uninty...

            //ViewBag.RouteId();



            // get all clubs
            ViewBag.footballClub = new SelectList(_PlayerRepository.GetAllClubs());
            // get all players
            var footballPlayers = _PlayerRepository.GetAllPlayers();

            // setup list of players chosen
            // IEnumerable<Player> chosenPlayers = new List<Player>();

            //filter to all players containing searchstring
            if (!String.IsNullOrEmpty(searchString))
            {
                //footballPlayers = footballPlayers.Where(s => s.Name.Contains(searchString)).ToList(); //don't need this unless footballplayers is a list
                footballPlayers = footballPlayers.Where(s => s.Name.Contains(searchString));
            }

            //chosenPlayers = footballPlayers.Take(1);

            //filter again to all players with same club
            if (string.IsNullOrEmpty(footballClub))
                return View(new SearchViewModel() { Players_SearchedFor = footballPlayers, Players_Chosen = new List<Player>() });
            else
            {
                return View(new SearchViewModel() { Players_SearchedFor = footballPlayers.Where(x => x.Club == footballClub), Players_Chosen = new List<Player>() });
            }

        }

        public PartialViewResult BlankPlayerRow(int id)
        {
            //var pr = new PlayerRepository();
            Player player = _PlayerRepository.GetPlayer(id);
            return PartialView("ChosenPlayerRow", player);
        }

    }
}
