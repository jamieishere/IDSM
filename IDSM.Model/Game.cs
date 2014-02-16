using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDSM.Model
{
    ///<summary>
    /// Game model
    /// Contains all information on a game
    ///</summary>
    ///<remarks>
    ///</remarks>
    public class Game
    {
        public Game()
        {
            UserTeams = new HashSet<UserTeam>();
        }
        
        //properties
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //[ForeignKey("Creator")]
        public int CreatorId { get; set; }
        public string Name { get; set; }
        public int CurrentOrderPosition { get; set; }
        public bool HasStarted { get; set; }
        public bool HasEnded { get; set; }
        public int WinnerId { get; set; }

        //navigation properties
        public virtual ICollection<UserTeam> UserTeams { get; set; }
        public virtual UserProfile Creator { get; set; }
    }
}
