using IDSM.Model;

namespace IDSM.Repository
{
    public interface IBanterRepository : IRepositoryBase<Banter>
    {
    }

    public class BanterRepository : RepositoryBase<Banter>, IBanterRepository
    {
        public BanterRepository(IDSMContext context) : base(context) { }
    }
}
