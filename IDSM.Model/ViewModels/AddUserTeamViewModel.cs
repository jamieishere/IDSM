
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IDSM.Model;


namespace IDSM.ViewModel
{
    /// <summary>
    /// AddUserTeamViewModel
    /// Used for the Game/ViewUsers view
    /// </summary>
    public class AddUserTeamViewModel
    {
        public IEnumerable<UserProfile> Users { get; set; }
        public Game Game { get; set; }
    }
}