using IDSM.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDSM.Logging.Services.Logging.Log4Net;
using IDSM.Exceptions;
using IDSM.Logging.Services.Logging;
using System.Transactions;
using System.Data.Common;

namespace IDSM.Repository
{
    public class UserTeamRepository : RepositoryBase<UserTeam>, IUserTeamRepository
    {
        public UserTeamRepository(IDSMContext context) : base(context) { }

        public IEnumerable<UserTeam> GetAllUserTeams()
        {
            var ut = DataContext.UserTeams.ToList();
            return ut;
        }

        public List<UserTeam> GetAllUserTeamsForGame(int gameId, string orderBy)
        {
            List<UserTeam> ut = new List<UserTeam>();
            switch (orderBy)
            {
                case "Id":
                    ut = DataContext.UserTeams.Where(t => t.GameId == gameId).OrderBy(o => o.Id).ToList();
                    break;
                case "OrderPosition":
                    ut = DataContext.UserTeams.Where(t => t.GameId == gameId).OrderBy(o => o.OrderPosition).ToList();
                    break;
            }
            return ut;
        }

        /// <summary>
        /// TryGet a UserTeam by UserTeamId, OR with GameID & UserID
        /// </summary>
        /// <param name="userTeamId"></param>
        /// <param name="gameId"></param>
        /// <param name="userId"></param>
        /// <param name="userTeam"></param>
        /// <returns>OperationStatus(Status=True) & UserTeam or OperationStatus(Status=False) & null</returns>
        public Boolean TryGetUserTeam(out UserTeam userTeam, int userTeamId = 0, int gameId = 0, int userId = 0)
        {
            userTeam = GetUserTeam(userTeamId, gameId, userId);
            if (userTeam == null) return false;
            return true;
        }

        /// <summary>
        /// Get a UserTeam by UserTeamId, OR with GameID & UserID
        /// </summary>
        /// <param name="userTeamId"></param>
        /// <param name="gameId"></param>
        /// <param name="userId"></param>
        /// <returns>UserTeam or null</returns>
        private UserTeam GetUserTeam(int userTeamId = 0, int gameId = 0, int userId = 0)
        {
          //  using (DataContext)
            //{
                UserTeam ut = (userTeamId != 0) ?
                    DataContext.UserTeams.Include("UserTeam_Players").SingleOrDefault(s => s.Id == userTeamId) :
                    DataContext.UserTeams.Include("UserTeam_Players").FirstOrDefault(s => s.GameId == gameId && s.UserId == userId);
                return ut;
           // }
        }

        /// <summary>
        /// Gets the UserTeam for a Game by the UserTeam's OrderPosition
        /// </summary>
        /// <param name="orderposition"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public UserTeam GetUserTeamByOrderPosition(int orderPosition, int gameId)
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

        // TODO:
        // Prevent duplicates being created.
        public OperationStatus SaveUserTeamPlayer(int userteamid, int  gameid, int pixelposy, int pixelposx, int playerid)
        {
            return SaveUTPlayer(userteamid, gameid, pixelposy, pixelposx, playerid);
        }


        public OperationStatus SaveUTPlayer(int userteamid, int gameid, int pixelposy, int pixelposx, int playerid)
        {
            UserTeam_Player player = new UserTeam_Player() { UserTeamId = userteamid, GameId = gameid, PixelPosX = pixelposx, PixelPosY = pixelposy, PlayerId = playerid };

                 var temp = DataContext.UserTeam_Players.Where(p => p.PlayerId == playerid && p.UserTeamId == userteamid).SingleOrDefault();

                // if userteam_player already exists, return true
                if (temp != null) return new OperationStatus { Status = true };

                try
                {
                    //unitOfWork.UserTeamRepository.
                    DataContext.UserTeam_Players.Add(player);
                    // if DataContext.Database.Connection.
                    //DataContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    return OperationStatus.CreateFromException("Error - SaveUserTeamPlayer.", ex);
                }
                return new OperationStatus { Status = true };
           // }
        }

        public OperationStatus SaveUTPlayer(int userteamid, int  gameid, int pixelposy, int pixelposx, int playerid, IDSMContext context)
        {
            UserTeam_Player player = new UserTeam_Player() { UserTeamId = userteamid, GameId = gameid, PixelPosX = pixelposx, PixelPosY = pixelposy, PlayerId = playerid };

            // using the base class (RepositoryBase) Get method.  Note how it is called.
           // var temp = base.Get<UserTeam_Player>(p => p.PlayerId == playerid && p.UserTeamId == userteamid);
            var temp = context.UserTeam_Players.SingleOrDefault(p => p.PlayerId == playerid && p.UserTeamId == userteamid);
 
            // if userteam_player already exists, return true
            if (temp != null) return new OperationStatus { Status = true };

            try
            {
                context.UserTeam_Players.Add(player);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                return OperationStatus.CreateFromException("Error - SaveUserTeamPlayer.", ex);
            }
            return new OperationStatus { Status = true };
        }

        public OperationStatus DeleteUserTeam(UserTeam userTeam)
        {
          //  using (DataContext)
           // {
                try
                {
                   // DataContext.UserTeams.Remove(userTeam); //can this cascade - delete all userteam players?  must set a foreign key.
                    //no, using remove like this doesn't work because the entity is not attached or something..
                    // need to better understand how deletion works....
                    //http://stackoverflow.com/questions/15637965/the-object-cannot-be-deleted-because-it-was-not-found-in-the-objectstatemanager
                    //http://stackoverflow.com/questions/1217052/entity-framework-delete-object-problem

                    //also chekc that the reason this does a cascade delete (deletes all the userteam players too), is beacause I set the cascade to true in IDSMContext (check if I remove it, if the cascade still happens)

                    DataContext.Entry(userTeam).State = System.Data.Entity.EntityState.Deleted;
                    DataContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    return OperationStatus.CreateFromException("Error deleting userteam.", ex);
                }
                return new OperationStatus { Status = true };
            //}
        }

        public OperationStatus SaveUserTeam(UserTeam team)
        {
          //  using (DataContext)
          //  {
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
           // }
        }

        ///// <summary>
        ///// Saves a UserTeam.  Requires the UserTeamId, GameId, and the list of UserTeam_Players to be saved.
        ///// TODO:
        ///// Save the team in a single transaction?
        ///// See this link for how to crate a TransactionScope & use .SaveChanges(false) then .AcceptAllChanges.
        ///// http://stackoverflow.com/questions/815586/entity-framework-using-transactions-or-savechangesfalse-and-acceptallchanges
        ///// </summary>
        ///// <param name="userTeamId"></param>
        ///// <param name="gameId"></param>
        ///// <param name="uTPlayers"></param>
        ///// <returns></returns>
        //public OperationStatus SaveUserTeam(int userTeamId, int gameId, IEnumerable<UserTeam_Player> uTPlayers)
        //{
        //    using (DataContext)
        //    {
        //        try
        //        {
        //            UserTeam _userTeam = null;
        //            OperationStatus _opStatus = null;

        //            // if we found a UserTeam, save the players
        //            if (TryGetUserTeam(out _userTeam, userTeamId: userTeamId))
        //            {
        //                // get ids of all UserTeam_Players for all UserTeams for this game
        //                var _utpids = from utp in DataContext.UserTeam_Players
        //                              where utp.GameId == gameId
        //                              select utp.PlayerId;
        //                List<int> temp = _utpids.ToList();

        //                foreach (UserTeam_Player m in uTPlayers)
        //                {
        //                    // if the UserTeam_Player does not already exist in this game, save the player
        //                    if (!temp.Any() || (temp.IndexOf(m.PlayerId) == -1))
        //                    {
        //                        _opStatus = SaveUserTeamPlayer(userTeamId, gameId, 1, 1, m.PlayerId);
        //                        if (!_opStatus.Status)
        //                        {
        //                            // if save fails for any reason, tell user to try again
        //                            return new OperationStatus { Status = false, Message = _opStatus.Message };
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                return new OperationStatus { Status = false, Message = _opStatus.Message};
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            // if save fails for any reason, tell user to try again
        //            return OperationStatus.CreateFromException("Could not save UserTeam.  Please try again.", ex);
        //        }
        //        return new OperationStatus { Status = true };
        //    }
        //}


        public OperationStatus CreateUserTeam(int userid, int gameid)
        {
          // //using (DataContext)
           // {
                var opStatus = new OperationStatus { Status = false };
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
               // Log4NetLogger logger = new Log4NetLogger();
               // logger.Info("Creating UserTeam: userid:"+userid+" gameid:"+gameid);

                return new OperationStatus { Status = true, OperationID = ut.Id};
           // }
        }

        

    }
}
