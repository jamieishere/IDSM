using System.Collections.Generic;
using IDSM.Model;
using IDSM.Model.ViewModels;
using IDSM.Repository;
using IDSM.ViewModel;

namespace IDSM.ServiceLayer
{
    public interface IService
    {
        void Save();

        IEnumerable<Game> GetAllGames();
        bool TryGetUserTeam(out UserTeam ut, int userTeamId);
        IEnumerable<string> GetAllClubs();
        OperationStatus CreateGame(int userId, string name);

        void StartGame(int gameId);
        void AddBanter(int userTeamId, string banter);
        void ResetGame(int gameId);
        void AddUserToGame(int userId, int gameId);
        AddUserTeamViewModel GetAddUserTeamViewModelForGame(Game game);
        ViewPlayersViewModel GetViewPlayersViewModel(int userTeamId, string footballClub, string searchString);
        OperationStatus AddUserTeamPlayer(int playerId, int userTeamId, int gameId);
    }
}
