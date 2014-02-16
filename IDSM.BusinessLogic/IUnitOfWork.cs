using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDSM.Repository;

namespace IDSM.ServiceLayer
{
    public interface IUnitOfWork:IDisposable
    {
        void Save();
        IDSMContext Context { get; }
        //IUserRepository UserRepository { get; }
        //IUserTeamRepository UserTeamRepository { get; }
        //IPlayerRepository PlayerRepository { get; }
        //IGameRepository GameRepository { get; }
    }

    //public interface IUnitOfWorkFactory
    //{
    //    IUnitOfWork CreateUnitOfWork();  
    //}
}
