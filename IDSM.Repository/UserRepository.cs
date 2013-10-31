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
    public class UserRepository : RepositoryBase<IDSMContext>, IUserRepository
    {
        /// <summary>
        /// GetAllUsers
        /// Gets all UserProfiles
        /// </summary>
        /// <returns>IEnumerable<UserProfile></returns>
        public IEnumerable<UserProfile> GetAllUsers()
        {
            using (DataContext)
            {
                var up = DataContext.UserProfiles.ToList();
                return up;
            }
        }

        public UserProfile GetUser(int userId)
        {
            using (DataContext)
            {
                UserProfile up = DataContext.UserProfiles.Where(u => u.UserId == userId).SingleOrDefault();
                return up;
            }
        }
    }
}
