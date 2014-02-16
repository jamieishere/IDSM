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
    /// UserTeam_Players model
    /// A userteam will have 1-11 userteam_players (1:M relationship)
    ///</summary>
    ///<remarks></remarks>
    public class UserTeam_Player
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //[ForeignKey("UserTeam")]
        public int UserTeamId { get; set; }
        public int GameId { get; set; }
       // [ForeignKey("Player")]
        public int PlayerId { get; set; }
        public int PixelPosX { get; set; }
        public int PixelPosY { get; set; }

        public virtual UserTeam UserTeam{ get; set; }
        public virtual Player Player { get; set; }
    }
}
