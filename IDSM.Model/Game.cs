using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public int CurrentOrderPosition { get; set; }
        public bool HasStarted { get; set; }
        public bool HasEnded { get; set; }
        public int WinnerId { get; set; }
        //public int [] UserTeamIds { get; set; }


        //navigation properties
        public virtual UserProfile Creator { get; set; }
        [ForeignKey("GameId")]
        public virtual ICollection<UserTeam> UserTeams { get; set; }
       // public virtual ICollection<UserProfile> Users { get; set; } // would be better to 'drill' into UserTeams to get Users, rather than do this?
    }
}
