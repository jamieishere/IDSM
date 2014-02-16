using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebMatrix.WebData;

namespace IDSM.Model
{
    ///<summary>
    /// UserTeam model
    ///</summary>
    ///<remarks></remarks>
    public class UserTeam
    {
        //constructor 
        public UserTeam()
        {
            UserTeam_Players = new HashSet<UserTeam_Player>();
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        // unecessary ... [ForeignKey("User")]
        public int UserId { get; set; }
        // unecessary ... [ForeignKey("Game")]
        public int GameId { get; set; }
        public int OrderPosition { get; set; }

        //navigation properties
        public virtual UserProfile User { get; set; }
        public virtual Game Game { get; set; }
        public virtual ICollection<UserTeam_Player> UserTeam_Players { get; set; }

    }
}
