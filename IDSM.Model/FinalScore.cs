using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDSM.Model
{
    ///<summary>
    /// FinalScore model
    /// On completion of a game (all users have selected 11 players for their userteam), the teams will be stored in the finalscore tables
    /// relationships
    /// game:finalscore 1:M
    /// user:finalscore 1:M
    /// GameID UserID GodScorePosition PlayerScorePosition BanterScore
    ///</summary>
    ///<remarks>
    /// May need to extend this so that we have a finalscore_userteam_players table/model that stores the userteam player details as they are on game completion in case player details are updated)
    ///</remarks>
    public class FinalScore
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public int UserId { get; set; }
        public int UserTeamId { get; set; }
        public int GodScorePosition { get; set; }
        public int PlayerScorePosition { get; set; }
        public int BanterScore { get; set; }
    }
}
