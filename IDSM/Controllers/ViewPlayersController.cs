using IDSM.Model;
using System.Collections.Generic;
using System.Web.Mvc;
using IDSM.Model.ViewModels;
using IDSM.ServiceLayer;

namespace IDSM.Controllers
{
    public class ViewPlayersController : Controller
    {
        private IService _service;
        private const int _teamSize = 1;

        public ViewPlayersController(IService service)
        {
            _service = service;
        }

        public ActionResult Index(int? userTeamId, string footballClub, string searchString)
        {
            // change this.
            // ViewPlayersViewModel needs to:
            // return selectlist rather than set viewbag here - see http://stackoverflow.com/questions/6623700/how-to-bind-a-selectlist-with-viewmodel
            // be modified to handle gameslist
            // checkout how accoutnataglance do multiple panels - want, ideally, for these panels - feed, gameslist, actual game, banter, to move around - have looked at the site itself and makes no sense... will need to watch the videos..
            // it's not necessary to have a jazzy inerface really.. but i should learn it, as planned.
            // however what i DO want... is to load the game info in by ajax instead of refresh
            // also, i need signlr to push??/ 

            if (userTeamId == null) userTeamId = 0;
                IEnumerable<string> _clubs = null;
                _clubs = _service.GetAllClubs();
                ViewBag.FootballClub = new SelectList(_clubs);

                return View(_service.GetViewPlayersViewModel((int)userTeamId, footballClub, searchString));
        }

        public ActionResult AddPlayer(int playerId, int userTeamId, int gameId)
        {
            ViewBag.Status = "Thre was a problem, player not added"; 
            if(_service.AddUserTeamPlayer(playerId, userTeamId, gameId).Status)
                ViewBag.Status = "Player added";

            return RedirectToAction("Index", new { userteamid = userTeamId });
        }

        public ActionResult AddBanter(int userTeamId, string banter)
        {
            _service.AddBanter(userTeamId, banter);
            return RedirectToAction("Index", new { userteamid = userTeamId });
        }
    }
}
