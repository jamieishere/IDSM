using IDSM.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDSM.Logging.Services.Logging.Log4Net;


namespace IDSM.Repository
{
    public class UserTeamRepository : RepositoryBase<IDSMContext>, IUserTeamRepository
    {

        public IEnumerable<UserTeam> GetAllUserTeams()
        {
            using (DataContext)
            {
                var ut = DataContext.UserTeams.ToList();
                return ut;
            }
        }

        public List<UserTeam> GetAllUserTeamsForGame(int gameId, string orderBy)
        {
            using (DataContext)
            {
                List<UserTeam> ut = new List<UserTeam>();
                switch (orderBy){
                    case "Id":
                        ut = DataContext.UserTeams.Where(t => t.GameId == gameId).OrderBy(o=>o.Id).ToList();
                        break;
                    case "OrderPosition":
                        ut = DataContext.UserTeams.Where(t => t.GameId == gameId).OrderBy(o=>o.OrderPosition).ToList();
                        break;
                }
                return ut;
            }
        }

        // Get a userteam base on UserTeamID, or based on GameID & UserID
        public UserTeam GetUserTeam(int userteamid = 0, int gameid = 0, int userid = 0)
        {
            using (DataContext)
            {
                var ut = (userteamid != 0) ?
                                            DataContext.UserTeams.Include("User").Include("Game").Include("UserTeam_Players").SingleOrDefault(s => s.Id == userteamid) :
                                            // had to change this from SingleOrDefault to FirstOrDefault because the query returned an error
                                            //  "Sequence has more then one element" - the table had multiple rows while I was testing with same gameid/userid
                                            //  this prevents it
                                            DataContext.UserTeams.Include("User").Include("Game").Include("UserTeam_Players").FirstOrDefault(s => s.GameId == gameid && s.UserId == userid);
                if (ut == null)
                {
                    // either return null or throw error not found.
                }
                if (ut is UserTeam)
                {
                    // do nothing
                }
                return ut;
            }
        }

        /// <summary>
        /// Gets the UserTeam for a Game by the UserTeam's OrderPosition
        /// </summary>
        /// <param name="orderposition"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public UserTeam GetUserTeamByOrderPosition(int orderPosition, int gameId)
        {
            using (DataContext)
            {
                var ut = DataContext.UserTeams.Include("User").SingleOrDefault(s => s.OrderPosition == orderPosition && s.GameId==gameId);
                if (ut == null)
                {
                    // either return null or throw error not found.
                }
                if (ut is UserTeam)
                {
                    // do nothing
                }
                return ut;
            }
        }




        public OperationStatus SaveUserTeamPlayer(int userteamid, int  gameid, int pixelposy, int pixelposx, int playerid)
        {
            using (DataContext)
            {
                UserTeam_Player player = new UserTeam_Player() { UserTeamId = userteamid, GameId = gameid, PixelPosX = pixelposx, PixelPosY = pixelposy, PlayerId = playerid };

                // does this userteam_player exist?
                // NOTE THIS CHECK IS NOW REDUNDANT AS WE DELETE THE WHOLE USER TEAM BEFORE ADDING IT AGAIN.
                // NOTE - IT IS NOT NOW REDUNDANT AS WE AREN'T USING THE SAVEUSERTEAM FUNCTION - WE ARE SAVING INDIVIDUAL USERPLAYERS TO DB everytime we click 'ADD'


                // NOTE: - finding entities:
                // http://msdn.microsoft.com/en-us/data/jj573936.aspx
                // using a query = a round trip to the database.  Can avoid this with .Find() but we don't have a primary key

                // using EF (this works)
                //var temp = DataContext.UserTeam_Players.FirstOrDefault(p => p.PlayerId == playerid &&
                //                            p.UserTeamId == userteamid);
                
                // using the base class (RepositoryBase) Get method.  Note how it is called.
                var temp = base.Get<UserTeam_Player>(p => p.PlayerId == playerid && p.UserTeamId == userteamid);
                    
                // if userteam_player exists, return
                if (temp != null) return new OperationStatus { Status = true };


                //NOTE!!!!!!!!!
                // NONE OF THE ABOVE IS NECESSARY
                // BECAUSE THERE IS A CHECK IN THE SAVEUSERTEAM METHOD AGAINST ALL EXISTING USERTEAMPLAYER IDS... 
                // THAT IS BETTER BECAUSE ITS ONLY 1 DB ACCESS, INSTEAD OF EVERY PLATYER



                try
                {
                    DataContext.UserTeam_Players.Add(player);
                    DataContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    return OperationStatus.CreateFromException("Error saving userteam player.", ex);
                }
                return new OperationStatus { Status = true };
            }
        }

        public OperationStatus DeleteUserTeam(UserTeam userTeam)
        {
            using (DataContext)
            {
                try
                {
                   // DataContext.UserTeams.Remove(userTeam); //can this cascade - delete all userteam players?  must set a foreign key.
                    //no, using remove like this doesn't work because the entity is not attached or something..
                    // need to better understand how deletion works....
                    //http://stackoverflow.com/questions/15637965/the-object-cannot-be-deleted-because-it-was-not-found-in-the-objectstatemanager
                    //http://stackoverflow.com/questions/1217052/entity-framework-delete-object-problem

                    //also chekc that the reason this does a cascade delete (deletes all the userteam players too), is beacause I set the cascade to true in IDSMContext (check if I remove it, if the cascade still happens)

                    DataContext.Entry(userTeam).State = System.Data.EntityState.Deleted;
                    DataContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    return OperationStatus.CreateFromException("Error deleting userteam.", ex);
                }
                return new OperationStatus { Status = true };
            }
        }

        public OperationStatus SaveUserTeam(UserTeam team)
        {
            using (DataContext)
            {
               // var opStatus = new OperationStatus { Status = false };

                try
                {
                    var ut = DataContext.UserTeams.Where(u => u.Id == team.Id).FirstOrDefault();

                    if (ut != null)
                    {
                        ut.GameId = team.GameId;
                        ut.OrderPosition = team.OrderPosition;
                        ut.UserId = team.UserId;
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

        public OperationStatus SaveUserTeam(int userteamid, int gameid, IEnumerable<UserTeam_Player> uTPlayers)
        //public OperationStatus SaveUserTeam(int userteamid, int gameid, IEnumerable<Player> players)
        {
            using (DataContext)
            {
                var opStatus = new OperationStatus { Status = false };
                try
                {
                    
                    // now i want to 


                    // There might be a better wya of doing this - using .contains, or icomparere or iequatable (objects)
                    //     see http://www.codeproject.com/Articles/20592/Implementing-IEquatable-Properly
                    // but it works for now
                    
                    // get current userteam players (to check against)
                    UserTeam _currentUserTeam = GetUserTeam(userteamid);
                    List<UserTeam_Player> UserTeam_Players = (List<UserTeam_Player>)_currentUserTeam.UserTeam_Players;

                    var utpids = from utp in DataContext.UserTeam_Players
                                 select utp.PlayerId;
                    List<int> temp = utpids.ToList();


                    //foreach (Player m in players)
                    foreach (UserTeam_Player m in uTPlayers)
                    {

                        //

                        // check UTPlayer doesn't already exist
                        //if (!UserTeam_Players.Contains(m))
                        // if tmep has any elements & if the current id is not in them
                        //if ((temp.IndexOf(m.Player.Id) == -1) && (temp.Any()))

                       // int test = temp.IndexOf(m.Player.Id);

                        if (!temp.Any() || (temp.IndexOf(m.Player.Id) == -1))
                        {
                            //opStatus = SaveUserTeamPlayer(userteamid, gameid, 1, 1, m.Id);
                            opStatus = SaveUserTeamPlayer(userteamid, gameid, 1, 1, m.Player.Id);
                            if (!opStatus.Status)
                            {
                                return OperationStatus.CreateFromException("Error saving userteam.", null);
                            }
                        }
                        //else if ((temp.IndexOf(m.Player.Id) == -1))
                        //{
                        //    //opStatus = SaveUserTeamPlayer(userteamid, gameid, 1, 1, m.Id);
                        //    opStatus = SaveUserTeamPlayer(userteamid, gameid, 1, 1, m.Player.Id);
                        //    if (!opStatus.Status)
                        //    {
                        //        return OperationStatus.CreateFromException("Error saving userteam.", null);
                        //    }
                        //}
                    }
                }
                catch (Exception ex)
                {
                    return OperationStatus.CreateFromException("Error saving userteam.", ex);
                }
                return new OperationStatus { Status = true };
            }
        }

        //public OperationStatus CreateUserTeam(User user, int gameid)

        public OperationStatus CreateUserTeam(int userid, int gameid)
        {
            using (DataContext)
            {
                UserTeam ut = new UserTeam() { UserId = userid, GameId = gameid };
                try
                {
                    DataContext.UserTeams.Add(ut);
                    DataContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    return OperationStatus.CreateFromException("Error creating userteam.", ex);
                }
                Log4NetLogger logger = new Log4NetLogger();
                logger.Info("Creating UserTeam: userid:"+userid+" gameid:"+gameid);

                return new OperationStatus { Status = true, OperationID = ut.Id};
            }
        }

        

    }
}
