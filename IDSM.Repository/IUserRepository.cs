using IDSM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDSM.Repository
{
    public interface IUserRepository : IRepositoryBase<UserProfile>
    {
        IEnumerable<UserProfile> GetAllUsers();
        Boolean TryGetUser(out UserProfile userProfile, int userId);
    }
}

