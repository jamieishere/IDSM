using IDSM.Model;
using System.Collections.Generic;
using System.Web.Mvc;
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

        public ActionResult Index(int userTeamId, string footballClub, string searchString)
        {
            UserTeam ut = null;
            if (!_service.TryGetUserTeam(out ut, userTeamId))
                return RedirectToAction("NotFound", "Error");
            IEnumerable<string> _clubs = null; 
            _clubs = _service.GetAllClubs();
            ViewBag.FootballClub = new SelectList(_clubs);

            return View(_service.GetViewPlayersViewModel(userTeamId, footballClub, searchString));
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
