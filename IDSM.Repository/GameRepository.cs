using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using IDSM.Logging.Services.Logging;
using IDSM.Logging.Services.Logging.Log4Net;
using IDSM.Model;
using IDSM.Repository.DTOs;

namespace IDSM.Repository
{
    /// <summary>
    /// GameRepository
    /// Contains all methods that access/manipulate Games within IDSMContext
    /// </summary>
    public class GameRepository : RepositoryBase<Game>, IGameRepository
    {
        public GameRepository(IDSMContext context) : base(context) { }

        /// GetAllGames
        /// Gets all Games, eager loads User (is this correct term?)
        /// </summary>
        /// <returns>IEnumerable<Game></returns>
        public IEnumerable<Game> GetAllGames()
        {      
            var _games = DataContext.Games
                    .Include(x => x.UserTeams)
                    .Include(x => x.UserTeams.Select(y => y.User))
                    .ToList();
            if (_games == null)
            {
                // log error
                return null;
            }
            return _games;
        }

        ///// <summary>
        ///// SaveGame
        ///// </summary>
        ///// <param name="creatorId"></param>
        ///// <param name="name"></param>
        ///// <returns>OperationStatus</returns>
        //public OperationStatus CreateGame(int creatorId, string name)
        //{
        //    Game game = new Game() { CreatorId = creatorId, Name = name };
        //    OperationStatus _opStatus =  new OperationStatus { Status = true };

        //    _opStatus = Create(game);

        //    DataContext.SaveChanges();
        //    return _opStatus;
        //}

        /// <summary>
        /// UpdateGame
        /// </summary>
        /// <param name="game"></param>
        /// <returns>OperationStatus</returns>
        public OperationStatus DoUpdateGame(Game game)
        {
            GameUpdateDTO _gameDto = new GameUpdateDTO();
            _gameDto = Mapper.Map(game, _gameDto);
            return Update(_gameDto, g => g.Id == _gameDto.Id);
        }
    }
}
