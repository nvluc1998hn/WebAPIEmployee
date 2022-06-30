using EmployeeManagement.Database.Repositories.Interfaces;
using EmployeeManagement.Database.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace EmployeeManagement.Database.Repositories.Implementations
{
    public class Repository<T> : IRepository<T>, IDisposable where T : class
    {
        private ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>Thêm mới 1 bản ghi</summary>
        /// <param name="entity">đầu vào là 1 object</param>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 6/27/2022 created
        /// </Modified>
        public virtual void Insert(T entity)
        {          
             _context.Set<T>().Add(entity);
        }

        /// <summary>Thêm danh sách bản ghi</summary>
        /// <param name="entitys">1 mảng object</param>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 6/27/2022 created
        /// </Modified>
        public virtual void Inserts(List<T> entitys)
        {
            _context.Set<T>().AddRange(entitys);
        }

        /// <summary>Tìm kiếm theo điều kiện lọc</summary>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 6/27/2022 created
        /// </Modified>
        public virtual IQueryable<T> FindAll(params Expression<Func<T, object>>[] includeProperties)
        {

            IQueryable<T> items = _context.Set<T>();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.Include(includeProperty);
                }
            }
            return items;
        }

        public virtual IQueryable<T> FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> items = _context.Set<T>();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.Include(includeProperty);
                }
            }
            return items.Where(predicate);
        }

        public virtual IQueryable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return _context.Set<T>().Where(where);
        }

        /// <summary>Tìm kiếm 1 bản ghi</summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 6/27/2022 created
        /// </Modified>
        public virtual T FindSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return FindAll(includeProperties).SingleOrDefault(predicate);
        }

        /// <summary>Tìm kiếm đồng bộ</summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 6/27/2022 created
        /// </Modified>
        public virtual async Task<T> FindSingleAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return await FindAll(includeProperties).SingleOrDefaultAsync(predicate).ConfigureAwait(false);
        }

        /// <summary>Xoá bản ghi khỏi danh sách</summary>
        /// <param name="entity">The entity.</param>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 6/27/2022 created
        /// </Modified>
        public virtual void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        /// <summary>Lấy dữ liệu của 1 bản ghi</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 6/27/2022 created
        /// </Modified>
        public virtual T GetById(object id)
        {
            return _context.Set<T>().Find(id);
        }

        /// <summary>Lấy tất cả bản ghi không qua điều kiện</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 6/27/2022 created
        /// </Modified>
        public virtual IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public virtual T Get(Expression<Func<T, bool>> where)
        {
            var entity = _context.Set<T>().Where(where).FirstOrDefault();
            return entity;
        }

        public virtual void Remove(object id)
        {
            var entity = GetById(id);
            Remove(entity);
        }

        public virtual void RemoveMultiple(List<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        /// <summary>Cập nhật thông tin</summary>
        /// <param name="entity">Đối tượng</param>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 6/27/2022 created
        /// </Modified>
        public virtual  void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.Set<T>().Update(entity);
        }

        /// <summary>Cập nhật nhìu thông tin</summary>
        /// <param name="entities">Danh sách đối tượng</param>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 6/27/2022 created
        /// </Modified>
        public virtual void Updates(List<T> entities)
        {
            _context.Set<T>().UpdateRange(entities);
        }

        public virtual void AddOrUpdateRange(List<T> entities, string columnName)
        {
            var set = _context.Set<T>();
            set.AddRange(entities);
        }

        /// <summary>Giải phóng dữ liệu</summary>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 6/27/2022 created
        /// </Modified>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_context != null && disposing)
            {
                _context.Dispose();
            }
        }

        public void SaveChange()
        {
            _context.SaveChanges();
        }
    }
}
