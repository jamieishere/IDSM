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
    public interface IUserRepository : IRepositoryBase<UserProfile>
    {
    }

    public class UserRepository : RepositoryBase<UserProfile>, IUserRepository
    {
        public UserRepository(IDSMContext context) : base(context) { }
    }
}
