
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IDSM.Models;
using IDSM.Model;


namespace IDSM.ViewModel
{
    public class ChosenTeamViewModel
    {
        public IEnumerable<Player> Players { get; set; }
        public int GameID { get; set; }
        public int UserTeamID { get; set; }
    }
}