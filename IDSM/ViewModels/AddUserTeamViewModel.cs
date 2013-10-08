
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IDSM.Models;
using IDSM.Model;


namespace IDSM.ViewModel
{
    public class AddUserTeamViewModel
    {
        public IEnumerable<UserProfile> Users { get; set; }
        public Game Game { get; set; }
    }
}