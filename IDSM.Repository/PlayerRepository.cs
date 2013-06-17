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

        public Player GetPlayer(int id)
        {
            using (DataContext)
            {
                var pl = DataContext.Players.SingleOrDefault(s => s.Id == id);
                if (pl == null)
                {
                    // either return null or throw error not found.
                }
                if (pl is Player)
                {
                    // do nothing
                }
                return pl;
            }
        }

        //public List<Player> GetAllPlayers() - use ienumerable BECAUSE... List implemenets Ienumerable... 
        //think turns into something you can access by index so certain important things arent available... 
        // AH - you can still do what you need to with a list, but it doesnt give the compiler a chance to optimise, so only use a list if you need to update
        // the list once you have it... in this case, we're just getting it... so it's better to be an IEnumerable
        public IEnumerable<Player> GetAllPlayers()
        {
            using (DataContext)
            {
                var pl = DataContext.Players.ToList();
                return pl;
            }
        }

        public IEnumerable<UserTeam_Player> GetAllChosenPlayers(int gameid)
        {
            using (DataContext)
            {
                //var pl = DataContext.UserTeam_Players.ToList();
                var chosenPlayers = from cp in DataContext.UserTeam_Players
                              where cp.GameId == gameid
                              select cp;
                return chosenPlayers;
            }
        }

        public IEnumerable<string> GetAllClubs()
        {
            using (DataContext)
            {
                var clubLst = new List<string>();
                var clubQry = from fp in DataContext.Players
                              orderby fp.Club
                              select fp.Club;
                clubLst.AddRange(clubQry.Distinct());

                return clubLst;
            }
        }

        public static OperationStatus UploadPlayersCSV(string filePath)
        {           
            return ProcessCSVHelper(filePath, new IDSMContext());           
        }

        public static OperationStatus ProcessCSVHelper(string filePath, IDSMContext DataContext)
        {
            using (DataContext)
            {
                //Set up our variables
                string Feedback = string.Empty;
                //FootballPlayerDBContext db2 = new FootballPlayerDBContext();
                StreamReader srCSV = new StreamReader(filePath);
                CsvReader csvReader = new CsvReader(srCSV);

                // NOTE:
                // Not sure if the 'ID' error on CSV import (that meant had to add the ID column to CSV)
                //      is coming from this line, or the for each loop.
                //      if was a real project would obviously investigate and fix!

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
                    //for each row in dt
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

                //Tidy Streameader up
                srCSV.Dispose();
                csvReader.Dispose();

                return new OperationStatus { Status = true };
            }
        }
    }
}
