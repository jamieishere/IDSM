using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace IDSM.Repository
{
    /// <summary>
    /// #1 Goal of RepositoryBase is expose the generic DbContext object & fire up a new instance of the DbContext.
    ///     Saves duplication of code (instantiating DbContext in every repository class) - 1 place for all repositories.
    ///     In addition contains common resuable methods (Get, GetList, Save, Update, StoredProc, Dispose)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    //public class RepositoryBase<C, T> : IDisposable
    //       where C : DbContext, IDisposedTracker, new()
    public class RepositoryBase<T> : IDisposable where T : class
    {
        protected IDSMContext DataContext;

        public RepositoryBase(IDSMContext context)
        {
            DataContext = context;
        }

        public virtual bool TryGet(out T entity, Expression<Func<T, bool>> predicate)
        {
            entity = Get(predicate);
            if (entity == null) return false;
            return true;
        }

        public virtual T Get(Expression<Func<T, bool>> predicate)
        {
            if (predicate != null)
                // potential exception - InvalidOperationException.  If want to catch this & throw a nicer user-friendly exception higher up, do so.
                return DataContext.Set<T>().Where(predicate).SingleOrDefault();

            throw new ApplicationException("Predicate value must be passed to Get<T>.");
        }

        public virtual T Get<TKey>(Expression<Func<T, bool>> predicate,
            Expression<Func<T, TKey>> orderBy)
        {
            if (predicate != null && orderBy != null)
                return DataContext.Set<T>().Where(predicate).SingleOrDefault();

            throw new ApplicationException("Predicate value and Order value must be passed to Get<T>.");
        }

        public virtual T Get(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            if (predicate == null || includeProperties == null) throw new ApplicationException("Predicate value and includeProperties must be passed to Get<T>.");
            foreach (var _property in includeProperties)
            {
                DataContext.Set<T>().Include(_property);
            }
            return DataContext.Set<T>().Where(predicate).FirstOrDefault();
        }

        public virtual IQueryable<T> GetList(Expression<Func<T, bool>> predicate)
        {
            if (predicate != null)
                return DataContext.Set<T>().Where(predicate);
            throw new ApplicationException("Predicate value must be passed to GetList<T>.");
        }

        public virtual IQueryable<T> GetList<TKey>(Expression<Func<T, bool>> predicate,
            Expression<Func<T, TKey>> orderBy)
        {
            return GetList(predicate).OrderBy(orderBy);
        }

        public virtual IQueryable<T> GetList<TKey>(Expression<Func<T, TKey>> orderBy)
        {
            return GetList().OrderBy(orderBy);
        }


        public virtual IQueryable<T> GetList(params Expression<Func<T, object>>[] includeProperties)
        {
            foreach (var _property in includeProperties)
            {
                DataContext.Set<T>().Include(_property);
            }
            return DataContext.Set<T>();
        }

        public virtual IQueryable<T> GetList()
        {
            return DataContext.Set<T>();
        }

        public virtual OperationStatus Create(T entity)
        {
            var _opStatus = new OperationStatus { Status = true };
            try
            {
                DataContext.Set<T>().Add(entity);
            }
            catch (Exception _exp)
            {

                _opStatus = OperationStatus.CreateFromException("Error creating " + typeof(T) + ".", _exp, true);
            }

            return _opStatus;
        }

        public OperationStatus Update(object dto, Expression<Func<T, bool>> currentEntityFilter) 
        {
            var _opStatus = new OperationStatus { Status = true };
            try
            {
                var _current = DataContext.Set<T>().FirstOrDefault(currentEntityFilter);
                DataContext.Entry(_current).CurrentValues.SetValues(dto);
            }
            catch (Exception _exp)
            {
                _opStatus = OperationStatus.CreateFromException("Error updating " + typeof(T) + ".", _exp, true);
            }
            return _opStatus;
        }

        public OperationStatus Update(object dto, params object[] keyValues)
        {
            var _opStatus = new OperationStatus { Status = true };
            try
            {
                var _current = DataContext.Set<T>().Find(keyValues);
                DataContext.Entry(_current).CurrentValues.SetValues(dto);
            }
            catch (Exception _exp)
            {
                _opStatus = OperationStatus.CreateFromException("Error updating " + typeof(T) + ".", _exp, true);
            }
            return _opStatus;
        }

        public virtual OperationStatus Delete(T entity)
        {
            var _opStatus = new OperationStatus { Status = true };
            try
            {
                // Deleted only removes current object, .Remove() deletes child objects too.
                //DataContext.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                DataContext.Set<T>().Remove(entity);

            }
            catch (Exception _exp)
            {
                return OperationStatus.CreateFromException("Error deleting " + typeof(T) + ".", _exp, true);
            }
            return _opStatus;
        }

        public void Dispose()
        {
            if (DataContext != null) DataContext.Dispose();
        }

        //public OperationStatus ExecuteStoreCommand(string cmdText, params object[] parameters)
        //{
        //    var _opStatus = new OperationStatus { Status = true };

        //    try
        //    {
        //        //opStatus.RecordsAffected = DataContext.ExecuteStoreCommand(cmdText, parameters);
        //        //TODO: [Papa] = Have not tested this yet.
        //        opStatus.RecordsAffected = DataContext.Database.ExecuteSqlCommand(cmdText, parameters);
        //    }
        //    catch (Exception exp)
        //    {
        //        OperationStatus.CreateFromException("Error executing store command: ", exp);
        //    }
        //    return opStatus;
        //}
    }
}
