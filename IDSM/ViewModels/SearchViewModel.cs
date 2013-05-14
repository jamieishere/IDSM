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
        public IEnumerable<Player> Players_Chosen { get; set; }
    }
}