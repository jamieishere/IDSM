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
    public interface IUserTeamRepository : IRepositoryBase<UserTeam>
    {
    }

    public class UserTeamRepository : RepositoryBase<UserTeam>, IUserTeamRepository
    {
        public UserTeamRepository(IDSMContext context) : base(context) { }
    }
}
