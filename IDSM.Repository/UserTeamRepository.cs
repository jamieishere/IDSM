using IDSM.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDSM.Repository
{
    public class UserTeamRepository : RepositoryBase<IDSMContext>, IUserTeamRepository
    {

        public UserTeam GetUserTeam(int userteamid)
        {
            using (DataContext)
            {
                var ut = DataContext.UserTeams.SingleOrDefault(s => s.Id == userteamid);
                if (ut == null)
                {
                    // either return null or throw error not found.
                }
                if (ut is UserTeam)
                {
                    // do nothing
                }
                return ut;
            }
        }


        public OperationStatus SaveUserTeamPlayer(int userteamid, int  gameid, int pixelposy, int pixelposx, int playerid)
        {
            using (DataContext)
            {
                UserTeam_Player player = new UserTeam_Player() { UserTeamId = userteamid, GameId = gameid, PixelPosX = pixelposx, PixelPosY = pixelposy, PlayerId = playerid };
                try
                {
                    DataContext.UserTeam_Players.Add(player);
                    DataContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    return OperationStatus.CreateFromException("Error saving userteam player.", ex);
                }
                return new OperationStatus { Status = true };
            }
        }

        //public OperationStatus CreateUserTeam(User user, int gameid)
        
        public OperationStatus CreateUserTeam(int userid, int gameid)
        {
            using (DataContext)
            {
                UserTeam ut = new UserTeam() { UserId = userid, GameId = gameid };
                try
                {
                    DataContext.UserTeams.Add(ut);
                    DataContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    return OperationStatus.CreateFromException("Error creating userteam.", ex);
                }
                return new OperationStatus { Status = true };
            }
        }

        public OperationStatus SaveUserTeam(int userteamid, int gameid, IEnumerable<Player> players)
        {
            using (DataContext)
            {
                var opStatus = new OperationStatus { Status = false };
                try
                {
                    foreach (Player m in players)
                    {
                        opStatus = SaveUserTeamPlayer(userteamid, gameid, 1, 1, m.Id);
                        if (!opStatus.Status)
                        {
                            return OperationStatus.CreateFromException("Error saving userteam.", null);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return OperationStatus.CreateFromException("Error saving userteam.", ex);
                }
                return new OperationStatus { Status = true };
            }
        }

    }
}
