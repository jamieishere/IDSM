using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebMatrix.WebData;

namespace IDSM.Model
{
    ///<summary>
    /// UserTeam model
    /// A user will have many userteams - 1 for each game they play in  (1:M relationship)
    ///</summary>
    ///<remarks>
    ///This is a test.
    ///</remarks>
    public class UserTeam
    {
        //constructor that populates the navigation properties
        public UserTeam()
        {
            //User = new UserProfile();
            // need to actively get this user.... need to somehow call a method.
            // this is a M:1 relationship (userteam has 1 user)...
           // User = new UserProfile(UserId);
        }

        //public UserTeam()
        //{
        //    User = new UserProfile();
        //}

        //primitive properties
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GameId { get; set; }
        public int OrderPosition { get; set; }

        //[ForeignKey("UserId")]
        public virtual UserProfile User { get; set; }
        public virtual Game Game { get; set; }
        //public virtual ICollection<UserTeam_Player> UserTeam_Players { get; set; }
        // had to change this to a list because coudn't cast from a Collection to a List - don't know why
        [ForeignKey("UserTeamId")]
        public virtual IList<UserTeam_Player> UserTeam_Players { get; set; }

    }
}
