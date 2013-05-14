using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDSM.Models;

namespace IDSM.Model
{
    ///<summary>
    /// Game model
    /// A game will have a name, have a creator and can have 1:M Userteams.
    ///</summary>
    ///<remarks>
    ///
    ///</remarks>
    public class Game
    {
        //constructor that populates the navigation properties
        public Game()
        {
            UserTeams = new HashSet<UserTeam>();
        }

        public int Id { get; set; }
        public int CreatorId { get; set; }
        public string Name { get; set; }

        //navigation properties
        public virtual UserProfile Creator { get; set; }
        public virtual ICollection<UserTeam> UserTeams { get; set; }
        
    }
}
