using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IDSM.Model;

namespace IDSM.ViewModel
{
    public class GameViewModel
    {
        public IEnumerable<Game> Games{ get; set; }
       
       // public string WinnerName { get; set; }
    }
}