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
    /// UserTeam model
    /// A user will have many userteams - 1 for each game they play in  (1:M relationship)
    ///</summary>
    ///<remarks>
    ///This is a test.
    ///</remarks>
    public class UserTeam
    {
        //primitive properties
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GameId { get; set; }

        //navigation properties - should these be virtual? to be overridden?
        //[ForeignKey("UserId")]
        public virtual UserProfile User { get; set; }
        public virtual Game Game { get; set; }
        public virtual ICollection<UserTeam_Player> UserTeam_Players { get; set; }
    }
}
