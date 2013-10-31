using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDSM.Logging.Services.Logging;
using IDSM.Logging.Services.Logging.Log4Net;
using IDSM.Model;

namespace IDSM.Repository
{
    /// <summary>
    /// GameRepository
    /// Contains all methods that access/manipulate Games within IDSMContext
    /// </summary>
    public class GameRepository : RepositoryBase<IDSMContext>, IGameRepository
    {
        /// <summary>
        /// GetGame
        /// Get single Game by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Game</returns>
        public Game GetGame(int id)
        {
            // using is the same as try/finally & automatically disposes of the datacontext on error
            using (DataContext)
            {
                var _game = DataContext.Games.SingleOrDefault(s => s.Id == id);
                if (_game == null)
                {
                    return null;
                }
                return _game;
            }
        }

        /// <summary>
        /// GetAllGames
        /// Gets all Games
        /// </summary>
        /// <returns>IEnumerable<Game></returns>
        public IEnumerable<Game> GetAllGames()
        {
            using (DataContext)
            {             
                var _games = DataContext.Games
                     .Include(x => x.UserTeams)
                     //.Include(x => x.UserTeams.Select(y => y.User))
                     .ToList();
                if (_games == null)
                {
                    // log error
                    return null;
                }
                return _games;
            }
        }

        /// <summary>
        /// SaveGame
        /// Creates/Adds a new Game object to the Context, saves to the database
        /// </summary>
        /// <param name="creatorId"></param>
        /// <param name="name"></param>
        /// <returns>OperationStatus</returns>
        /// <remarks>
        /// TODO:
        /// Update logger so it takes a single opStatus OperationStatus object as parameter.
        /// Requires updating logging project.
        /// </remarks>
        public OperationStatus CreateGame(int creatorId, string name)
        {
            using (DataContext)
            {
                Game game = new Game() { CreatorId = creatorId, Name = name };
                try
                {
                    DataContext.Games.Add(game);
                    DataContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    OperationStatus _opStatus = OperationStatus.CreateFromException("Error creating Game. CreatorId = "+creatorId+", name = "+name, ex);
                    ILogger _logger = LogFactory.Logger();
                    _logger.Error(_opStatus.Message, ex);
                    return _opStatus;
                }
                return new OperationStatus { Status = true };
            }
        }

        /// <summary>
        /// UpdateGame
        /// Takes a game object, gets same object from Context, manually maps all properties to avoid mass assignment security issues, calls .SaveChanges()
        /// </summary>
        /// <param name="game"></param>
        /// <returns>OperationStatus</returns>
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
                    OperationStatus _opStatus = OperationStatus.CreateFromException("Error updating Game, Id="+ game.Id, ex);
                    Log4NetLogger _logger = new Log4NetLogger();
                    _logger.Error(_opStatus.Message, ex);
                    return _opStatus;
                }
                return new OperationStatus { Status = true };
            }
        }
    }
}
