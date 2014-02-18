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
using IDSM.Logging.Services.Logging.Log4Net;
using AutoMapper;
using IDSM.Logging.Services.Logging;
using System.Transactions;
using System.Text;
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

        /// <summary>
        /// Return View displaying current ChosenPlayers for the current UserTeam, plus the filtered list (depending on selected club/searchstring) of Players.
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
            IEnumerable<string> _clubs = null; 
            _clubs = _service.GetAllClubs();
            ViewBag.FootballClub = new SelectList(_clubs);

            return View(_service.GetUserTeamViewModel(userTeamId, footballClub, searchString));
        }

        /// <summary>
        /// Adds the selected player to the UserTeam
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="userTeamId"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public ActionResult AddPlayer(int playerId, int userTeamId, int gameId)
        {
            if(AddUserTeamPlayer(playerId, userTeamId, gameId))
            {
                ViewBag.Status = "Player added";
            }
            else
            { 
                ViewBag.Status = "Thre was a problem, player not added"; 
            }

            return RedirectToAction("Index", new { userteamid = userTeamId });
        }

        private bool AddUserTeamPlayer(int playerId, int userTeamId, int gameId)
        {
            Player _player = null;

            // check a Player exists for this playerId
            if (!_service.TryGetPlayer(out _player, playerId)) return false;

            try
            {
                    // save the UserTeam_Player
               // OperationStatus _op = _userTeamRepository.SaveUTPlayerAndUpdateGame(userTeamId, gameId, 1, 1, playerId);
                OperationStatus _op = _service.SaveUTPlayerAndUpdateGame(userTeamId, gameId, 1, 1, playerId);
                _service.Save();
            }
            catch (Exception ex)
            {
                ILogger _logger = LogFactory.Logger();
                _logger.Error(String.Format("AddUserTeamPlayer failed. playerId:{0}, userTeamId:{1}, gameId:{2}", playerId, userTeamId, gameId), ex);
                return false;
            }
            return true;
        }
    }
}
