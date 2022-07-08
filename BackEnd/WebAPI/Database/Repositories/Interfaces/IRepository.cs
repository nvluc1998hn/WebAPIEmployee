using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Database.Repositories.Interfaces
{
    public interface IRepository<T,Key> where T : class
    {
        T FindSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        IQueryable<T> FindAll(params Expression<Func<T, object>>[] includeProperties);

        IQueryable<T> FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        IQueryable<T> GetMany(Expression<Func<T, bool>> where);

        Key Insert(T entity, string keyField);

      

        void Update(T entity);

        void Updates(List<T> entities);

        void AddOrUpdateRange(List<T> entities, string idColumn);

        void Remove(T entity);

        void Remove(object id);

        IQueryable<T> GetAll();

        T Get(Expression<Func<T, bool>> where);

        T GetById(object id);

        void RemoveMultiple(List<T> entities);

        void SaveChange();
    }
}
