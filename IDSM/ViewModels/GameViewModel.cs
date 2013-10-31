using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IDSM.Model;

namespace IDSM.ViewModel
{
    /// <summary>
    /// GameViewModel
    /// Used for the Games/Index view
    /// Holds all games 
    /// </summary>
    public class GameViewModel
    {
        public IEnumerable<Game> Games{ get; set; }
       
       // public string WinnerName { get; set; }
    }
}