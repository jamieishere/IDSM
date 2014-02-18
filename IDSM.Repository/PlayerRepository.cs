using CsvHelper;
using IDSM.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Web;
using System.Configuration;

namespace IDSM.Repository
{
    public interface IPlayerRepository : IRepositoryBase<Player>
    {
    }

    public class PlayerRepository : RepositoryBase<Player>, IPlayerRepository
    {
        public PlayerRepository(IDSMContext context) : base(context) { }
    }
}
