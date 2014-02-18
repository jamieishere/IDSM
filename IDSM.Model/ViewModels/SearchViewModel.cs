using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IDSM.Model;

namespace IDSM.ViewModel
{
    /// <summary>
    /// SearchViewModel
    /// Used on the ViewPlayers/Index view
    /// Holds full players searched for list, list of players chosen, plus all details on the userteam.
    /// </summary>
    public class SearchViewModel
    {
        //public IEnumerable<Player> Players_SearchedFor { get; set; }
        public IEnumerable<PlayerDto> Players_SearchedFor { get; set; }
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

    public class PlayerDto : Player
    {
        public bool HasBeenChosen { get; set; }
    }
}