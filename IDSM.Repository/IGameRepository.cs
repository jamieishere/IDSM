using IDSM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDSM.Repository
{
    public interface IGameRepository : IRepositoryBase<Game>
    {
        //Game GetGame(int gameid);
       // Boolean TryGetGame(out Game game, int gameId);
        IEnumerable<Game> GetAllGames();
      //  OperationStatus CreateGame(int creatorid, string name);
        OperationStatus DoUpdateGame(Game game);
    }
}

