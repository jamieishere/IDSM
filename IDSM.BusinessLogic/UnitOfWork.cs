using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDSM.Model;
using IDSM.Repository;

namespace IDSM.ServiceLayer
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private IDSMContext _context;

        public UnitOfWork()
        {
           _context = new IDSMContext();
        }

        public IDSMContext Context
        {
            get {return _context;}
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    //public class UnitOfWorkFactory : IUnitOfWorkFactory
    //{
    //    private readonly IUnityContainer _container;

    //    public UnitOfWorkFactory(IUnityContainer container)
    //    {
    //        _container = container;
    //    }

    //    public IUnitOfWork CreateUnitOfWork()
    //    {
    //        return _container.Resolve<UnitOfWork>();
    //    }
    //}
}