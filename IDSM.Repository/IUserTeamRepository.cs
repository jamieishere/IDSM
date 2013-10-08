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

        UserTeam GetUserTeam(int userTeamId, int gameId, int userId);
        List<UserTeam> GetAllUserTeamsForGame(int gameid, string orderBy);
        IEnumerable<UserTeam> GetAllUserTeams();
        UserTeam GetUserTeamByOrderPosition(int orderPosition, int gameId);
        OperationStatus CreateUserTeam(int userId, int gameId);
        OperationStatus DeleteUserTeam(UserTeam userTeam);
        //OperationStatus SaveUserTeam(int userteamid, int gameid, IEnumerable<Player> players);
        OperationStatus SaveUserTeam(UserTeam team);
        OperationStatus SaveUserTeam(int userTeamId, int gameId, IEnumerable<UserTeam_Player> userTeamPlayers);

        //UserTeam_Player GetUserTeamPlayer(int userteamplayerid);
        //List<UserTeam_Player> GetAllUserTeamPlayers(int userteamid);
        OperationStatus SaveUserTeamPlayer(int userTeamId, int  gameId, int pixelPosY, int pixelPosX, int playerId);

        //Banter GetBanterItem(int banterid);
        //List<Banter> GetAllBanterForThisUserTeam(int userteamid);
        //List<Banter> GetAllBanterForThisGame(int gameid);
    }
}
