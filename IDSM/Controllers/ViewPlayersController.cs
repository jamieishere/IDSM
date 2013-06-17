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
        IUserTeamRepository _userRepository;
        IPlayerRepository _playerRepository;

        ///<summary>
        /// ViewPlayers constructor
        ///</summary>
        ///<remarks>
        ///</remarks>
        public ViewPlayersController(IUserTeamRepository userRepo, IPlayerRepository playerRepo)
        {
            //_UserRepository = userRepo ?? ModelContainer.Instance.Resolve<UserRepository>();
            //_playerRepository = playerRepo ?? ModelContainer.Instance.Resolve<PlayerRepository>();
            _userRepository = userRepo;
            _playerRepository = playerRepo;
        }

        [HttpPost]
        public ActionResult Index(ChosenTeamViewModel ct)
        {
            var OpStatus = new OperationStatus() { Status = false };
            ViewBag.FootballClub = new SelectList(_playerRepository.GetAllClubs());

            
// so now, when a user goes to the game, chosenplayers is populated on load from the DB
// when reading out players for the search list, check if they are in the (chosenlist)
//--- if they are it needs to be greyed out..


            //Get UserID  WebMatrix.WebData.WebSecurity.CurrentUserId
            //Old way - Guid userGuid = (Guid)Membership.GetUser().ProviderUserKey;  
            // also, set a cache value so don't have to hit the Db everytime Cache.Add(User.Identity.Name, user.UserID); // Key: Username; Value: Guid.

            //i actually I don't need userID here, I need to return a viewmodel  so i can have the players and the userteam id...  not sure how to populate from the view tho
            //userteam could be in the viewbag?  no, viewmodel is probaly better...
 
            // BUT... how do i create the viewmodel in BeginFormCollection?  Perhaps I just create a viewmodel that has players, userid, gameid as a poperty and then add those as hidden fields in the form?
            //  even if this works, i still don't get how the form elements that BeginFormCollection lays down are needed. Why the first one players.index???

            // OK THIS WORKED.  MVC basically figures out from the formcollection how to bind to a viewmodel.  nice.  however still dont get why that players.index is needed.  what happens to it on binging?  does it just get discarded?
                
            OpStatus = _userRepository.SaveUserTeam(ct.GameID, ct.UserTeamID, ct.Players);

            if (!OpStatus.Status)
            {
                ViewBag.OperationStatus = OpStatus;
            }
            return View(new SearchViewModel() { Players_SearchedFor = _playerRepository.GetAllPlayers(), Players_Chosen = new List<Player>() });
        }

        public ActionResult Index(string footballClub, string searchString, int gameid)
        {
            //var pr = new PlayerRepository();

            //probably needs refactoring to handle errors better in the get methods, also i need to undertsand IList/List
            // for example - its' better to use IList - why exactly? i guess because you can switch out for 
            // or ienumerable/iqueryable..... 
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


            // get all clubs
            ViewBag.FootballClub = new SelectList(_playerRepository.GetAllClubs());
            // get all players
            var FootballPlayers = _playerRepository.GetAllPlayers();


            // i've got userteam players here.
            // i am only interested in the list of Ids
            // I don't actually want to exclude these from the list - I want to 'mark' them
            // I need to create a viewmodel from players (use automapper) and add 1 field (alreadychosen)
            // so, would be better to get an array of the usedplayerids, then loop through and where the playerid in FootballPlayers mathc, update the chosen value to true.
            // i would prob do this with a for loop, but i bet there is a way to do it in linq... 
            // maybe post my loop to codereview or to rob.....
            //
            //http://stackoverflow.com/questions/183791/how-would-you-do-a-not-in-query-with-linq

            //
            var ChosenPlayers = _playerRepository.GetAllChosenPlayers(gameid);

            // need to use the ecept on the list on chosenplayers_playerid / players_id

            //

           // var answer = list1.Except(list2);


            // setup list of players chosen
            // IEnumerable<Player> chosenPlayers = new List<Player>();

            //filter to all players containing searchstring
            if (!String.IsNullOrEmpty(searchString))
            {
                //footballPlayers = footballPlayers.Where(s => s.Name.Contains(searchString)).ToList(); //don't need this unless footballplayers is a list
                FootballPlayers = FootballPlayers.Where(s => s.Name.Contains(searchString));
            }

            //chosenPlayers = footballPlayers.Take(1);

            //filter again to all players with same club
            if (string.IsNullOrEmpty(footballClub))
                return View(new SearchViewModel() { Players_SearchedFor = FootballPlayers, Players_Chosen = new List<Player>() });
            else
            {
                return View(new SearchViewModel() { Players_SearchedFor = FootballPlayers.Where(x => x.Club == footballClub), Players_Chosen = new List<Player>() });
            }

        }

        public PartialViewResult BlankPlayerRow(int id)
        {
            //var pr = new PlayerRepository();
            Player player = _playerRepository.GetPlayer(id);
            return PartialView("ChosenPlayerRow", player);
        }

    }
}
