using CsvHelper;
using IDSM.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Web;
using System.Configuration;

namespace IDSM.Repository
{
    public class PlayerRepository : RepositoryBase<IDSMContext>, IPlayerRepository
    {
        public PlayerRepository(IDSMContext context) : base(context) { }

        public Boolean TryGetPlayer(out Player player, int playerId)
        {
            player = GetPlayer(playerId);
            if (player == null) return false;
            return true;
        }

        public Boolean nTryGetPlayer(out Player player, int playerId)
        {
            player = nGetPlayer(playerId);
            if (player == null) return false;
            return true;
        }

        public Player nGetPlayer(int playerId)
        {
           // using (DataContext)
         //   {
                Player pl = DataContext.Players.SingleOrDefault(s => s.Id == playerId);
                if (pl == null)
                {
                    return null;
                }
                return pl;
            //}
        }

        /// <summary>
        /// GetPlayer
        /// Gets a Player by Id
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns>Player</returns>
        public Player GetPlayer(int playerId)
        {
          //  using (DataContext)
          //  {
                Player pl = DataContext.Players.SingleOrDefault(s => s.Id == playerId);
                if (pl == null)
                {
                    return null;
                }
                return pl;
            //}
        }

        /// <summary>
        /// GetAllPlayers
        /// Gets all Players
        /// </summary>
        /// <returns>IEnumerable<Player></returns>
        public IEnumerable<Player> GetAllPlayers()
        {
          //  using (DataContext)
          //  {
                var pl = DataContext.Players.ToList();
                return pl;
         //   }
        }

        /// <summary>
        /// GetAllChosenPlayerIdsForGame
        /// Gets int array of all the Ids of all the UserTeam_Players currently selected by UserTeams for specific Game
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns>int[]</returns>
        /// <remarks>
        /// Returning an array gives marginal performance benefit in this situation 
        /// http://stackoverflow.com/questions/434761/array-versus-listt-when-to-use-which
        /// </remarks>
        public int[] GetAllChosenPlayerIdsForGame(int gameId) 
        {
          //  using (DataContext)
          //  {
                //var pl = DataContext.UserTeam_Players.ToList();
                var _chosenPlayers = from cp in DataContext.UserTeam_Players
                              where cp.GameId == gameId
                              select cp.PlayerId;
                return _chosenPlayers.ToArray();
           // }
        }

        /// <summary>
        /// GetAllChosenPlayersForGame
        /// Gets all the UserTeam_Players currently selected by UserTeams for specific Game
        /// </summary>
        /// <param name="gameid"></param>
        /// <returns>IEnumerable<UserTeam_Player></returns>
        public IEnumerable<UserTeam_Player> GetAllChosenPlayersForGame(int gameid)
        {
           // using (DataContext)
          //  {
                var chosenPlayers = DataContext.UserTeam_Players.Include("Player").ToList().Where(p => p.GameId ==gameid);
                return chosenPlayers.ToList();
           // }
        }

        /// <summary>
        /// GetAllChosenPlayersForUserTeam
        /// Gets all the UserTeam_Players currently selected by a specific UserTeam
        /// </summary>
        /// <param name="userTeamId"></param>
        /// <returns>IEnumerable<UserTeam_Player></returns>
        public IEnumerable<UserTeam_Player> GetAllChosenPlayersForUserTeam(int userTeamId)
        {
         //   using (DataContext)
         //   {
                //var chosenPlayers = iGetAllChosenPlayersForUserTeam(userTeamId, DataContext);
                var chosenPlayers = DataContext.UserTeam_Players.Include("Player").ToList().Where(p => p.UserTeamId == userTeamId);
                return chosenPlayers.ToList();
          //  }
        }

        /// <summary>
        /// GetAllChosenPlayersForUserTeam
        /// Gets all the UserTeam_Players currently selected by a specific UserTeam
        /// </summary>
        /// <param name="userTeamId"></param>
        /// <returns>IEnumerable<UserTeam_Player></returns>
        public IEnumerable<UserTeam_Player> nGetAllChosenPlayersForUserTeam(int userTeamId)
        {
                var chosenPlayers = iGetAllChosenPlayersForUserTeam(userTeamId, DataContext);
                return chosenPlayers.ToList();
        }


        /// <summary>
        /// GetAllChosenPlayersForUserTeam
        /// Gets all the UserTeam_Players currently selected by a specific UserTeam
        /// </summary>
        /// <param name="userTeamId"></param>
        /// <returns>IEnumerable<UserTeam_Player></returns>
        public IEnumerable<UserTeam_Player> iGetAllChosenPlayersForUserTeam(int userTeamId, IDSMContext context)
        {
                var chosenPlayers = context.UserTeam_Players.Include("Player").ToList().Where(p => p.UserTeamId == userTeamId);
                return chosenPlayers.ToList();
        }

        /// <summary>
        /// GetAllClubs
        /// Gets a distinct list of all the football clubs in the Player database
        /// </summary>
        /// <returns>IEnumerable<string></returns>
        public IEnumerable<string> GetAllClubs()
        {
       //     using (DataContext)
        //    {
                var clubLst = new List<string>();
                var clubQry = from fp in DataContext.Players
                              orderby fp.Club
                              select fp.Club;
                clubLst.AddRange(clubQry.Distinct());

                return clubLst;
          //  }
        }

        /// <summary>
        /// UploadPlayersCSV
        /// Takes a .csv file containing Player data in the correct model format, inserts into the database
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>OperationStatus</returns>
        public static OperationStatus UploadPlayersCSV(string filePath)
        {           
            return ProcessCSVHelper(filePath, new IDSMContext());           
        }

        /// <summary>
        /// ProcessCSVHelper
        /// Takes a .csv file containing Player data in the correct model format, inserts into the database
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="DataContext"></param>
        /// <returns>OperationStatus</returns>
        public static OperationStatus ProcessCSVHelper(string filePath, IDSMContext DataContext)
        {
           // using (DataContext)
           // {
                string Feedback = string.Empty;
                StreamReader srCSV = new StreamReader(filePath);
                CsvReader csvReader = new CsvReader(srCSV);

                // NOTE:
                //      'ID' error on CSV import is either is coming from this line, or the for each loop below.
                //       Temporarily fixed by adding an ID column to CSV
                List<Player> FootballPlayerList = new List<Player>();

                try
                {
                    FootballPlayerList = new List<Player>(csvReader.GetRecords<Player>());
                }
                catch (Exception ex)
                {
                    return OperationStatus.CreateFromException("Error reading from CSV.", ex);
                }

                try
                {
                    foreach (Player m in FootballPlayerList)
                    {
                        DataContext.Players.Add(m);
                    }
                    DataContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    return OperationStatus.CreateFromException("Error saving players to DB from CSV.", ex);
                }

                srCSV.Dispose();
                csvReader.Dispose();

                return new OperationStatus { Status = true };
           // }
        }
    }
}
