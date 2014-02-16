using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq.Expressions;
using IDSM.Model;

namespace IDSM.Repository
{
    public interface IRepositoryBase<T> where T : class
    {
        T Get(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetList(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetList<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> orderBy);
        IQueryable<T> GetList<TKey>(Expression<Func<T, TKey>> orderBy);
        IQueryable<T> GetList();
        OperationStatus Create(T entity);
        //OperationStatus Update(T entity, params string[] propsToUpdate);
        OperationStatus Save();
     //   OperationStatus Update(T entity, string idColName, params string[] propsToUpdate);
        OperationStatus Update(object dto, Expression<Func<T, bool>> currentEntityFilter);
       // OperationStatus Delete(T entity);
    }
}
