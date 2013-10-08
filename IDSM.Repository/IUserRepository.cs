using IDSM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDSM.Repository
{
    public interface IUserRepository
    {
        IEnumerable<UserProfile> GetAllUsers();
        UserProfile GetUser(int userId);
    }
}

