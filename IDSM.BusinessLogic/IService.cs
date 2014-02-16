using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDSM.Model;
using IDSM.Repository;

namespace IDSM.ServiceLayer
{
    public interface IService
    {
        // Repositories
        IUserRepository Users { get; }
        IUserTeamRepository UserTeams { get; }
        IPlayerRepository Players { get; }
        IGameRepository Games { get; }
        IUserTeam_PlayerRepository UserTeamPlayers { get; }

        void Save();

        // Game methods
        OperationStatus CreateGame(int creatorId, string name);
        Game GetGame(int id);
        IEnumerable<Game> GetAllGames();
        Boolean TryGetGame(out Game game, int id);
        OperationStatus UpdateGame(int gameId, int userTeamId, int _teamSize);
        OperationStatus SaveUTPlayerAndUpdateGame(int userTeamId, int gameId, int pixelposy, int pixelposx, int playerId);
        int CalculateWinner(int gameId);

        //UserTeam methods
    }
}
