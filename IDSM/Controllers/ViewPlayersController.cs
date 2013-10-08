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


namespace IDSM.Controllers
{
    public class ViewPlayersController : Controller
    {
        IUserRepository _userRepository;
        IUserTeamRepository _userTeamRepository;
        IPlayerRepository _playerRepository;
        IGameRepository _gameRepository;

        private const int _numPlayersInTeam = 1;

        ///<summary>
        /// ViewPlayers constructor
        ///</summary>
        ///<remarks>
        ///</remarks>
        public ViewPlayersController(IUserRepository userRepo, IPlayerRepository playerRepo, IGameRepository gameRepo, IUserTeamRepository userTeamRepo)
        {
            //_UserRepository = userRepo ?? ModelContainer.Instance.Resolve<UserRepository>();
            //_playerRepository = playerRepo ?? ModelContainer.Instance.Resolve<PlayerRepository>();
            _userRepository = userRepo;
            _playerRepository = playerRepo;
            _gameRepository = gameRepo;
            _userTeamRepository = userTeamRepo;
        }

       

        /// <summary>
        /// Index ActionResult
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
            var FootballPlayers = _playerRepository.GetAllPlayers(); //(IList<Player>)





            // Need to map the player model to a viewmodel that includes the property 'HasBeenChosen'
            // Need to get the ChosenPlayers from the database (only need IDs)
            // Need to find all the players in FootballPlayers who's ID is in the ChosenPlayers list/array
            // and set HasBeenChosen to true

            // Either do a forloop (but for 100,000+players that's a lot
            // Or... do it a different way...
            // ask stack.
            //var result = GetMyIEnumerable()
            //   .ToList();
            //result.ForEach(x => x.property1 = 100);

            // THIS IS HOW TO DO IT
            //http://stackoverflow.com/questions/2984045/linq-c-where-foreach-using-index-in-a-list-array

            //http://stackoverflow.com/questions/1924535/c-any-benefit-of-listt-foreach-over-plain-foreach-loop

            //
            //http://stackoverflow.com/questions/183791/how-would-you-do-a-not-in-query-with-linq

            UserTeam ut = new UserTeam();
            ut = _userTeamRepository.GetUserTeam(userTeamId,0,0);
            // 
            int[] ChosenPlayerIDs = _playerRepository.GetAllChosenPlayerIdsForGame(ut.GameId);

            Game g = _gameRepository.GetGame(ut.GameId);

            //IEnumerable<Player> ChosenPlayers = new List<Player>();
            IEnumerable<UserTeam_Player> ChosenPlayers = _playerRepository.GetAllChosenPlayersForUserTeam(ut.Id);
           
            //why aren't players accessible from chosenplayers?
            //maybe they are
            // don't understand the difference between .Include and ... joins

            // if i want to change this to get FootballPLayers - ChosenPlayers, then I need to change the GetAllChosenPlayers to return IList<Player> instead of 
            // IList<UserTeam_Player> - should probably do this anyway - the method name is misleading currently.
            // however, if i do this I can't 'blank out' players in the view to show they have been chosen, they just wouldn't appear in the list.
            // if i DO want to do this, use .Except:
            //var finalplayers = FootballPlayers.Except(ChosenPlayers);

            // do i want to foreach over the football players or the chosen playerss???
            // how will i decide
            // i want to see if the current footballplayerid is in the chosen player list

            //for (int i = 0; i < ChosenPlayers.Length; i++)
            //  //  someNames.setInfo(i, "blah");
            //}

           // for(int i = 0; i < FootballPlayers.Length; i++){
            foreach (Player p in FootballPlayers){
                // someNames.setInfo(i, "blah");
                if (ChosenPlayerIDs.Contains(p.Id))
                {
                    p.HasBeenChosen = true;
                }
            }


            // need to use the ecept on the list on chosenplayers_playerid / players_id

            //

           // var answer = list1.Except(list2);


            // setup list of players chosen
           

            //filter to all players containing searchstring
            if (!String.IsNullOrEmpty(searchString))
            {
                //footballPlayers = footballPlayers.Where(s => s.Name.Contains(searchString)).ToList(); //don't need this unless footballplayers is a list
                FootballPlayers = FootballPlayers.Where(s => s.Name.Contains(searchString));
            }


            //get game.currentorderposition
            //get userteam.currentorder
            //get the userteam via orderposition and gameid
            string _addedPlayerMessage = "Current player is {0}.  There are {1} turns left before your go.";
            
            //@Model.ActiveUserName.  There are @Model.TurnsLeft before your go." + ut.User.UserName;
            string _tmpActiveUserName;
            int _tmpTurnsLeft = 0;

            if (g.HasEnded)
            {
                _addedPlayerMessage = "The game has ended.";
            }
            else
            {
                List<UserTeam> uts = _userTeamRepository.GetAllUserTeamsForGame(g.Id, "Id");

                //get activeuserteam
                if (ut.OrderPosition != g.CurrentOrderPosition)
                {
                    UserTeam _activeUt = _userTeamRepository.GetUserTeamByOrderPosition(g.CurrentOrderPosition, g.Id);
                    _tmpActiveUserName = _activeUt.User.UserName;
                    switch (_activeUt.OrderPosition > ut.OrderPosition)
                    {
                        case true:
                            _tmpTurnsLeft = (ut.OrderPosition == uts.Count) ? 1 : _activeUt.OrderPosition - ut.OrderPosition;
                            break;
                        case false:
                            _tmpTurnsLeft = (ut.OrderPosition == uts.Count) ? 1 : ut.OrderPosition - _activeUt.OrderPosition;
                            break;
                    }

                    _addedPlayerMessage = String.Format(_addedPlayerMessage, _tmpActiveUserName, _tmpTurnsLeft);
                }
                else
                {
                   // _tmpActiveUserName = ut.User.UserName;

                }
            }

            //chosenPlayers = footballPlayers.Take(1);

            //filter again to all players with same club
            if (string.IsNullOrEmpty(footballClub))
                return View(new SearchViewModel() { Players_SearchedFor = FootballPlayers, Players_Chosen = ChosenPlayers, GameId = ut.GameId, GameName=g.Name, GameCurrentOrderPosition=g.CurrentOrderPosition, UserTeamId = ut.Id, UserName = ut.User.UserName, UserTeamOrderPosition= ut.OrderPosition, AddedPlayerMessage = _addedPlayerMessage});
            else
            {
                return View(new SearchViewModel() { Players_SearchedFor = FootballPlayers.Where(x => x.Club == footballClub), Players_Chosen = ChosenPlayers, GameId = ut.GameId, GameName = g.Name, GameCurrentOrderPosition = g.CurrentOrderPosition, UserTeamId = ut.UserId, UserName = ut.User.UserName, UserTeamOrderPosition = ut.OrderPosition, AddedPlayerMessage = _addedPlayerMessage});
            }

        }

        public ActionResult AddPlayer(int id, int userteamid, int gameId)
        //public PartialViewResult BlankPlayerRow(int id, int userteamid, int gameId)
        {
            var pr = new PlayerRepository();
            Player player = _playerRepository.GetPlayer(id);

            //create a userteam player using this player
            UserTeam_Player utplayer = new UserTeam_Player();
            utplayer.PlayerId = id;
            utplayer.Player = player;

            //    //***********************************************************
            //    // TODO:
            //    // Prevent duplicates being created.
            //    var ut = new UserTeamRepository();
            //    ut.SaveUserTeamPlayer(userteamid, gameId, 1, 1, id);
            //    //***********************************************************
            var ut = new UserTeamRepository();
            OperationStatus op = ut.SaveUserTeamPlayer(userteamid, gameId, 1, 1, id);

            //update game currentorderposition

            //add player.
            //if operationstatus = true
            //need to move game on 1 turn.
            //get currentorderposition
            //get totalteamcount
            //get playersinteamcount
            //get maxplayercount

            //check if we have a winner
            // if we are on the last team, AND playersinteamcount=maxplayercount
                // if ((currentorderposition+1) == totalteamcount) && 
            //move currentoderposition +1 
            //unless
            //currentorderposition+1 = teamcount
            //inwhich case it's 0 (backtostart)

            //now check if we have winner
            //winner if currenorderposition, teamcoutn, teamplayercount=maxplayercount

            Game game = _gameRepository.GetGame(gameId);
            List<UserTeam> uts = _userTeamRepository.GetAllUserTeamsForGame(gameId, "Id");
            int _utCount = uts.Count;
            List<UserTeam_Player> utps = (List<UserTeam_Player>)_playerRepository.GetAllChosenPlayersForUserTeam(userteamid);
            if ((utps.Count == _numPlayersInTeam) && (game.CurrentOrderPosition+1 == _utCount))
            {
                game.HasEnded = true;
                game.WinnerId = CalculateWinner(gameId);
                game.CurrentOrderPosition = -10; //arbitrary integer to ensure it is no userteam's 'go'.

            }
            else
            {
                int _currentOrderPosition = game.CurrentOrderPosition;
                game.CurrentOrderPosition = ((_currentOrderPosition + 1) == uts.Count) ? 0 : _currentOrderPosition + 1;
            }





            // if number of chosenplayers for this userteam = numplayersinteam, and this is the last userteam, calculate the winner
           

            _gameRepository.UpdateGame(game);

            return RedirectToAction("Index", new { userteamid = userteamid });
            //return Redirect("Index", new { userteamid = userteamid });
        }

        

        ///<summary>
        /// CalculateWinner
        ///</summary>
        ///<remarks>
        /// takes gameid, gets userteams, runs through each userteam, calculates score for each, selects winner, returns id
        ///</remarks>
        private int CalculateWinner(int gameId)
        {
            //topscore = list TopScores
            int _tempScore;
            TopScore _topScore = new TopScore();
            List<UserTeam> _userTeams = _userTeamRepository.GetAllUserTeamsForGame(gameId,"Id");
            List<UserTeam_Player> _players = new List<UserTeam_Player> { };

            foreach (UserTeam team in _userTeams)
            {
                _players = (List<UserTeam_Player>)_playerRepository.GetAllChosenPlayersForUserTeam(team.Id);
                _tempScore = 0;
                foreach (UserTeam_Player player in _players)
                {
                    _tempScore = _tempScore + player.Player.Age;
                }
                if (_tempScore > _topScore._score)
                {
                    _topScore._userTeamId = team.Id;
                    _topScore._score = _tempScore;
                }
            }
            return _topScore._userTeamId;
        }

        protected struct TopScore
        {
            public int _userTeamId{get;set;}
            public int _score { get; set; }
        }


        ///<summary>
        /// ViewPlayers Index action
        ///<remarks>
        /// Unused action - previously used in conjunction with BlankPlayerRow & ChosenTeamViewModel - when user submitted player choises, the posted form values would be auto bound to the ChosenTeamViewModel and saved to the database.
        ///</remarks>
        //[HttpPost]
        //public ActionResult Index(ChosenTeamViewModel ct)
        //{
        //    var OpStatus = new OperationStatus() { Status = false };
        //    ViewBag.FootballClub = new SelectList(_playerRepository.GetAllClubs());

        //    Debug.Assert(ct.UserTeam_Players != null);
        //    OpStatus = _userTeamRepository.SaveUserTeam(ct.UserTeamID, ct.GameID, ct.UserTeam_Players);

        //    if (!OpStatus.Status)
        //    {
        //        ViewBag.OperationStatus = OpStatus;
        //    }

        //    return RedirectToAction("Index", "ViewPlayers", new { userTeamId = ct.UserTeamID });
        //}

        /////<summary>
        ///// ViewResult BlankPlayerRow
        /////<remarks>
        ///// Unused.  Utilised HTMLPrefix Extension method BeginCollectionItem to create a dynamic list.
        /////</remarks>
        //public PartialViewResult BlankPlayerRow(int id, int userteamid, int gameId)
        //{
        //    var pr = new PlayerRepository();
        //    Player player = _playerRepository.GetPlayer(id);
        
        //    //create a userteam player using this player
        //    UserTeam_Player utplayer = new UserTeam_Player();
        //    utplayer.PlayerId = id;
        //    utplayer.Player = player;

        //    //***********************************************************
        //    // TODO:
        //    // Prevent duplicates being created.
        //    var ut = new UserTeamRepository();
        //    ut.SaveUserTeamPlayer(userteamid, gameId, 1, 1, id);
        //    //***********************************************************

        //    //update game currentorderposition
        //    Game game = _gameRepository.GetGame(gameId);
        //    List<UserTeam> uts = _userTeamRepository.GetAllUserTeamsForGame(gameId, "Id");
        //    int _utCount = uts.Count;
        //    int _currentOrderPosition = game.CurrentOrderPosition;
        //    game.CurrentOrderPosition = ((_currentOrderPosition + 1) == uts.Count) ? 0 : _currentOrderPosition + 1;

        //    List<UserTeam_Player> utps = (List<UserTeam_Player>)_playerRepository.GetAllChosenPlayersForUserTeam(userteamid);

        //    if ((utps.Count == _numPlayersInTeam) && (game.CurrentOrderPosition == _utCount - 1))
        //    {
        //        game.HasEnded = true;
        //        game.WinnerId = CalculateWinner(gameId);
        //    }

        //    _gameRepository.UpdateGame(game);
        //    return PartialView("ChosenPlayerRow", utplayer);
        //}

    }
}
