using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDSM.Model
{
    ///<summary>
    /// UserTeam_Players model
    /// A userteam will have 1-11 userteam_players (1:M relationship)
    ///</summary>
    ///<remarks>
    ///This is a test.
    ///</remarks>
    public class UserTeam_Player
    {
        public int Id { get; set; }
        public int UserTeamId { get; set; }
        public int GameId { get; set; }
        public int PlayerId { get; set; }
        public int PixelPosX { get; set; }
        public int PixelPosY { get; set; }

        public virtual UserTeam UserTeam{ get; set; }
    }
}
