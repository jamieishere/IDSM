using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDSM.Model
{
    /// <summary>
    /// Banter
    /// Class containing all the back and forth banter between Users
    /// </summary>
    public class Banter
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int UserId { get; set; }
        public int GameId { get; set; }
        public string BanterText { get; set; }
        public int Votes { get; set; }
    }
}
