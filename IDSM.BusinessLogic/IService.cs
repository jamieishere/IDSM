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
       // IUserRepository Users { get; }
       // IUserTeamRepository UserTeams { get; }
        //IPlayerRepository Players { get; }
       // IGameRepository Games { get; }
       // IUserTeam_PlayerRepository UserTeamPlayers { get; }

        void Save();

        // Game methods
        OperationStatus CreateGame(int creatorId, string name);
        OperationStatus StartGame(int gameId); 
        OperationStatus ResetGame(int gameId);
        OperationStatus AddUserToGame(int userId, int gameId);
        OperationStatus DoUpdateGame(Game game);
        Game GetGame(int id);
        IEnumerable<Game> GetAllGames();
        Boolean TryGetGame(out Game game, int id);
        OperationStatus UpdateGame(int gameId, int userTeamId, int _teamSize);
        OperationStatus SaveUTPlayerAndUpdateGame(int userTeamId, int gameId, int pixelposy, int pixelposx, int playerId);
        int CalculateWinner(int gameId);

        // User methods
        UserProfile GetUser(int userId);
        IEnumerable<UserProfile> GetAllUsers();
        Boolean TryGetUser(out UserProfile userProfile, int userId);

        //UserTeam methods
        UserTeam GetUserTeam(int userTeamId = 0, int gameId = 0, int userId = 0);
        Boolean TryGetUserTeam(out UserTeam userTeam, int userTeamId = 0, int gameId = 0, int userId = 0);
        List<UserTeam> GetAllUserTeamsForGame(int gameid, string orderBy);
        IEnumerable<UserTeam> GetAllUserTeams();
        UserTeam GetUserTeamByOrderPosition(int orderPosition, int gameId);
        int[] GetAllChosenUserTeamPlayerIdsForGame(int gameId);
        IEnumerable<UserTeam_Player> GetAllChosenUserTeamPlayersForGame(int gameid);
        IEnumerable<UserTeam_Player> GetAllChosenUserTeamPlayersForTeam(int userTeamId);
        OperationStatus CreateUserTeam(int userId, int gameId);
        OperationStatus DeleteUserTeam(UserTeam userTeam);
        OperationStatus SaveUserTeam(UserTeam team);
        OperationStatus SaveUserTeamPlayer(int userTeamId, int gameId, int pixelPosY, int pixelPosX, int playerId);

        //Player methods
        IEnumerable<string> GetAllClubs();
        Boolean TryGetPlayer(out Player player, int playerId);
        Player GetPlayer(int playerId);
        IEnumerable<Player> GetAllPlayers();
    }
}
