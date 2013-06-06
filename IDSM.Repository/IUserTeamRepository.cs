using IDSM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDSM.Repository
{
    public interface IUserTeamRepository
    {
        //User GetUser(int userid);
        //List<User> GetAllUsersForGame(int gameid);

        UserTeam GetUserTeam(int userteamid, int userid, int gameid);
        //List<UserTeam> GetAllUserTeamsForGame(int gameid);
        OperationStatus CreateUserTeam(int userid, int gameid);
        OperationStatus SaveUserTeam(int userteamid, int gameid, IEnumerable<Player> players);

        //UserTeam_Player GetUserTeamPlayer(int userteamplayerid);
        //List<UserTeam_Player> GetAllUserTeamPlayers(int userteamid);
        OperationStatus SaveUserTeamPlayer(int userteamid, int  gameid, int pixelposy, int pixelposx, int playerid);

        //Banter GetBanterItem(int banterid);
        //List<Banter> GetAllBanterForThisUserTeam(int userteamid);
        //List<Banter> GetAllBanterForThisGame(int gameid);
    }
}
