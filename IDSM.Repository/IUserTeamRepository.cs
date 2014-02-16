using IDSM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDSM.Repository
{
    public interface IUserTeamRepository : IRepositoryBase<UserTeam>
    {
        Boolean TryGetUserTeam(out UserTeam userTeam, int userTeamId = 0, int gameId = 0, int userId = 0);
        List<UserTeam> GetAllUserTeamsForGame(int gameid, string orderBy);
        IEnumerable<UserTeam> GetAllUserTeams();
        UserTeam GetUserTeamByOrderPosition(int orderPosition, int gameId);
        OperationStatus CreateUserTeam(int userId, int gameId);
        OperationStatus DeleteUserTeam(UserTeam userTeam);
        OperationStatus SaveUserTeam(UserTeam team);
        OperationStatus SaveUTPlayer(int userteamid, int gameid, int pixelposy, int pixelposx, int playerid);
        OperationStatus SaveUserTeamPlayer(int userTeamId, int  gameId, int pixelPosY, int pixelPosX, int playerId);
    }
}
