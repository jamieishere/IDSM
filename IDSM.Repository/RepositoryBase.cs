using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using IDSM.Logging.Services.Logging.Log4Net;
using IDSM.Model;
using IDSM.Repository.DTOs;

namespace IDSM.Repository
{
    /// <summary>
    /// #1 Goal of RepositoryBase is expose the generic DbContext object & fire up a new instance of the DbContext.
    ///     Saves duplication of code (instantiating DbContext in every repository class) - 1 place for all repositories.
    ///     In addition contains common resuable methods (Get, GetList, Save, Update, StoredProc, Dispose)
    /// </summary>
    /// <typeparam name="C"></typeparam>
    //public class RepositoryBase<C, T> : IDisposable
    //       where C : DbContext, IDisposedTracker, new()
    public class RepositoryBase<T> : IDisposable where T : class
    {
        protected IDSMContext DataContext;

        public RepositoryBase(IDSMContext context)
        {
            this.DataContext = context;
        }

        public virtual T Get(Expression<Func<T, bool>> predicate)
        {
            if (predicate != null)
            {
                return DataContext.Set<T>().Where(predicate).SingleOrDefault();
            }
            else
            {
                throw new ApplicationException("Predicate value must be passed to Get<T>.");
            }
        }

        public virtual T Get<TKey>(Expression<Func<T, bool>> predicate,
            Expression<Func<T, TKey>> orderBy)
        {
            if (predicate != null)
            {
                return DataContext.Set<T>().Where(predicate).SingleOrDefault();
            }
            else
            {
                throw new ApplicationException("Predicate value must be passed to Get<T>.");
            }
        }

        public virtual T Get(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            foreach (var property in includeProperties)
            {
                DataContext.Set<T>().Include(property);
            }
            return DataContext.Set<T>().Where(predicate).FirstOrDefault();
        }

        public virtual IQueryable<T> GetList(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return DataContext.Set<T>().Where(predicate);
            }
            catch (Exception ex)
            {
                //Log error
            }
            return null;
        }

        public virtual IQueryable<T> GetList<TKey>(Expression<Func<T, bool>> predicate,
            Expression<Func<T, TKey>> orderBy)
        {
            try
            {
                return GetList(predicate).OrderBy(orderBy);
            }
            catch (Exception ex)
            {
                //Log error
            }
            return null;
        }

        public virtual IQueryable<T> GetList<TKey>(Expression<Func<T, TKey>> orderBy)
        {
            try
            {
                return GetList().OrderBy(orderBy);
            }
            catch (Exception ex)
            {
                //Log error
            }
            return null;
        }


        public virtual IQueryable<T> GetList(params Expression<Func<T, object>>[] includeProperties)
        {
            try
            {
                foreach (var property in includeProperties)
                {
                    DataContext.Set<T>().Include(property);
                }
                return DataContext.Set<T>();
            }
            catch (Exception ex)
            {
                //Log error
            }
            return null;
        }

        public virtual IQueryable<T> GetList()
        {
            try
            {
                return DataContext.Set<T>();
            }
            catch (Exception ex)
            {
                //Log error
            }
            return null;
        }

        public virtual OperationStatus Create(T entity)
        {
            try
            {
                DataContext.Set<T>().Add(entity);
            }
            catch (Exception exp)
            {
                //Log error
                return new OperationStatus { Status = false };
            }

            return new OperationStatus { Status = true };
        }

        public OperationStatus Update(object dto, Expression<Func<T, bool>> currentEntityFilter) 
        {
            OperationStatus _opStatus = new OperationStatus { Status = true };
            try
            {
                var current = DataContext.Set<T>().FirstOrDefault(currentEntityFilter);
                DataContext.Entry(current).CurrentValues.SetValues(dto);
            }
            catch (Exception exp) {
                // prob want this in the service really when call save changes.
                _opStatus = OperationStatus.CreateFromException("Error updating " + typeof(T) + ".", exp);
                Log4NetLogger _logger = new Log4NetLogger();
                _logger.Error(_opStatus.Message, exp);
                return _opStatus;
            }
            return _opStatus;
        }

        public void Update(object dto, params object[] keyValues)
        {
            var current = DataContext.Set<T>().Find(keyValues);
            DataContext.Entry(current).CurrentValues.SetValues(dto);
        }

        public virtual OperationStatus Save()
        {
            OperationStatus opStatus = new OperationStatus { Status = true };

            try
            {
                opStatus.Status = DataContext.SaveChanges() > 0;
            }
            catch (Exception exp)
            {
                opStatus = OperationStatus.CreateFromException("Error saving " + typeof(T) + ".", exp);
            }

            return opStatus;
        }

        

        public OperationStatus ExecuteStoreCommand(string cmdText, params object[] parameters)
        {
            var opStatus = new OperationStatus { Status = true };

            try
            {
                //opStatus.RecordsAffected = DataContext.ExecuteStoreCommand(cmdText, parameters);
                //TODO: [Papa] = Have not tested this yet.
                opStatus.RecordsAffected = DataContext.Database.ExecuteSqlCommand(cmdText, parameters);
            }
            catch (Exception exp)
            {
                OperationStatus.CreateFromException("Error executing store command: ", exp);
            }
            return opStatus;
        }

        public virtual OperationStatus Delete(T entity)
        {
            OperationStatus opStatus = new OperationStatus { Status = true };

            try
            {
                // Deleted only removes current object, .Remove() deletes child objects too.
                //DataContext.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                DataContext.Set<T>().Remove(entity);

            }
            catch (Exception exp)
            {
                return OperationStatus.CreateFromException("Error deleting " + typeof(T), exp);
            }

            return opStatus;
        }


        public void Dispose()
        {
            if (DataContext != null) DataContext.Dispose();
        }
    }
}
