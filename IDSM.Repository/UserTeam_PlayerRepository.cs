using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDSM.Model;

namespace IDSM.Repository
{
    public interface IUserTeam_PlayerRepository : IRepositoryBase<UserTeam_Player>
    {
    }
    public class UserTeam_PlayerRepository: RepositoryBase<UserTeam_Player>, IUserTeam_PlayerRepository
    {
        public UserTeam_PlayerRepository(IDSMContext context) : base(context) { }
    }
}
