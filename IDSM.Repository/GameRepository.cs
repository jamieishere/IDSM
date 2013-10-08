using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
                //var gm = DataContext.Games.Include("UserTeams").ToList();
              //  var gm = DataContext.Games.Include(ut => ut.UserTeams).Include("Users").Where.ToList();#
                //var gm = DataContext.Games.Include("UserTeams").Include("Users").Where(u=>u.UserTeams..ToList();


                // what is different nomenclature for this - using string, over using lambda?
               // var gm = DataContext.Games.Include("UserTeams.User").ToList();
                
                var gm = DataContext.Games
                     .Include(x => x.UserTeams)
                     //.Include(x => x.UserTeams.Select(y => y.User).Where(z => z.UserId == 1))
                     .Include(x => x.UserTeams.Select(y => y.User))
                     .ToList();

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

        public OperationStatus UpdateGame(Game game)
        {
            using (DataContext)
            {
                try
                {
                    var gm = DataContext.Games.Where(g => g.Id == game.Id).FirstOrDefault();

                    if (gm != null)
                    {
                        gm.CreatorId = game.CreatorId;
                        gm.CurrentOrderPosition = game.CurrentOrderPosition;
                        gm.HasStarted = game.HasStarted;
                        gm.Name = game.Name;
                        gm.HasEnded = game.HasEnded;
                        gm.WinnerId = game.WinnerId;
                        DataContext.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    return OperationStatus.CreateFromException("Error updating userteam.", ex);
                }
                return new OperationStatus { Status = true };
            }
        }
    }
}
