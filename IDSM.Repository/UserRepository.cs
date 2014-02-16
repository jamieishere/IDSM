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
    public class UserRepository : RepositoryBase<UserProfile>, IUserRepository
    {
        public UserRepository(IDSMContext context) : base(context) { }

        public IEnumerable<UserProfile> GetAllUsers()
        {
            var up = DataContext.UserProfiles.ToList();
            return up;
        }

        public Boolean TryGetUser(out UserProfile userProfile, int userId)
        {
            userProfile = Get(u => u.UserId == userId);
            if(userProfile==null) return false;
            return true;
        }
    }
}
