using IDSM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDSM.Repository
{
    public interface IGameRepository
    {
        Game GetGame(int gameid);
        IEnumerable<Game> GetAllGames();
        OperationStatus SaveGame(int creatorid, string name);

        //Game GetFinalScore(int finalscoreid);
        //List<Game> GetAllFinalScoresForGame(int gameid);
    }
}

