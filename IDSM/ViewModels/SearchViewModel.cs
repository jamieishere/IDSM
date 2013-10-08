using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IDSM.Models;
using IDSM.Model;

namespace IDSM.ViewModel
{
    public class SearchViewModel
    {
        public IEnumerable<Player> Players_SearchedFor { get; set; }
        public IEnumerable<UserTeam_Player> Players_Chosen { get; set; }
        //public IEnumerable<Player> Players_Chosen { get; set; }
        public int GameId { get; set; }
        public string GameName { get; set; }
        public int GameCurrentOrderPosition { get; set; }
        public int UserTeamId { get; set; }
        public string UserName { get; set; }
        public int UserTeamOrderPosition { get; set; }

        public string AddedPlayerMessage { get; set; }
    }
}