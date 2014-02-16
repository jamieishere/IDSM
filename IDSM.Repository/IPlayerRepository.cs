using IDSM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDSM.Repository
{
    public interface IPlayerRepository
    {
        Player GetPlayer(int playerid);
        Boolean TryGetPlayer(out Player player, int playerid);
        Boolean nTryGetPlayer(out Player player, int playerid);
        IEnumerable<Player> GetAllPlayers();
        IEnumerable<string> GetAllClubs();
        IEnumerable<UserTeam_Player> GetAllChosenPlayersForGame(int gameid);
        //Boolean TryGetAllChosenPlayersForUserTeam(out IEnumerable<UserTeam_Player> utPlayers, int userTeamId);
        IEnumerable<UserTeam_Player> GetAllChosenPlayersForUserTeam(int userTeamId);
        IEnumerable<UserTeam_Player> nGetAllChosenPlayersForUserTeam(int userTeamId);
        IEnumerable<UserTeam_Player> iGetAllChosenPlayersForUserTeam(int userTeamId, IDSMContext context);
        int[] GetAllChosenPlayerIdsForGame(int gameid);
    }
}
