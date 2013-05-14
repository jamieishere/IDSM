using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDSM.Model;

namespace IDSM.Repository
{
    public class GameRepository : RepositoryBase<IDSMContext>, IGameRepository
    {
        public Game GetGame(int id)
        {
            using (DataContext)
            {
                var gm = DataContext.Games.SingleOrDefault(s => s.Id == id);
                if (gm == null)
                {
                    // either return null or throw error not found.
                }
                if (gm is Game)
                {
                    // do nothing
                }
                return gm;
            }
        }

        public IEnumerable<Game> GetAllGames()
        {
            using (DataContext)
            {
                var gm = DataContext.Games.ToList();
                return gm;
            }
        }

        public OperationStatus SaveGame(int creatorid, string name)
        {
            using (DataContext)
            {
                Game game = new Game() { CreatorId = creatorid, Name = name };
                try
                {
                    DataContext.Games.Add(game);
                    DataContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    return OperationStatus.CreateFromException("Error saving game.", ex);
                }
                return new OperationStatus { Status = true };
            }
        }
    }
}
