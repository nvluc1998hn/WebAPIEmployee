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
using Microsoft.Extensions.Logging;


namespace EmployeeManagement.Database.Repositories.Implementations
{
    /// <summary>
    ///   Thực hiện các chức năng thêm,sửa,xóa,tìm kiếm
    /// </summary>
    /// <typeparam name="T">Đối tượng truyền vào</typeparam>
    /// <typeparam name="Key">Khóa chính của bảng</typeparam>
    /// <Modified>
    /// Name Date Comments
    /// lucnv 08-07-2022 created
    /// </Modified>
    public class Repository<T, Key> : IRepository<T, Key>, IDisposable where T : class
    {
        private ApplicationDbContext _context;
        private readonly ILogger<T> _logger;

        public Repository(ApplicationDbContext context, ILogger<T> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>Hàm thêm mới</summary>
        /// <param name="entity">Đối tượng truyền vào</param>
        /// <param name="keyField">Khóa chính của bảng</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 08-07-2022 created
        /// </Modified>
        public virtual Key Insert(T entity, string keyField)
        {
            // Sau khi insert thành công sẽ sinh ra ID ngược lại không có
            Key key = default;
            try
            {
                var data = _context.Set<T>().Add(entity);
                _context.SaveChanges();
                if (data != null)
                {
                    var keyTemp = data.Entity.GetType().GetProperty(keyField).GetValue(data.Entity);
                    if (keyTemp != null && !string.IsNullOrEmpty(keyTemp.ToString()))
                    {
                        key = (Key)keyTemp;
                    }
                }
            }
            catch (Exception ex)
            {

                _logger.LogError("Insert" + ex.Message);
            }
            return key;
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
            try
            {
                if (includeProperties != null)
                {
                    foreach (var includeProperty in includeProperties)
                    {
                        items = items.Include(includeProperty);
                    }
                }
            }
            catch (Exception ex) 
            {

                _logger.LogError("FindAll" + ex.Message);
            }         
            return items;
        }

        /// <summary>Tìm kiếm theo điều kiện lọc.</summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 10-07-2022 created
        /// </Modified>
        public virtual IQueryable<T> FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> items = _context.Set<T>();
            try
            {
                if (includeProperties != null)
                {
                    foreach (var includeProperty in includeProperties)
                    {
                        items = items.Include(includeProperty);
                    }
                }
            }
            catch (Exception ex)
            {

                _logger.LogError("FindAll" + ex.Message);
            }        
            return items.Where(predicate);
        }  
        
        public virtual IQueryable<T> GetMany(Expression<Func<T, bool>> where)
        {
            IQueryable<T> data = _context.Set<T>();
            try
            {
                data = data.Where(where);
            }
            catch (Exception ex)
            {

                _logger.LogError("GetMany" + ex.Message);
            }
            return data;
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
            T t = null;
            try
            {
                t = FindAll(includeProperties).SingleOrDefault(predicate);
            }
            catch (Exception ex)
            {
                _logger.LogError("FindSingle" + ex.Message);
            }
            return t;
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
            try
            {
                _context.Set<T>().Remove(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError("Remove" + ex.Message);
            }
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
            T data = null;
            try
            {
                data = _context.Set<T>().Find(id);
                
            }
            catch (Exception ex)
            {

                _logger.LogError("GetById" + ex.Message);
            }
            return data;
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

        /// <summary>Lấy dữ liệu</summary>
        /// <param name="where">The where.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 11-07-2022 created
        /// </Modified>
        public virtual T Get(Expression<Func<T, bool>> where)
        {         
            return _context.Set<T>().Where(where).FirstOrDefault();
        }

        /// <summary>Xóa 1 bản ghi</summary>
        /// <param name="id">The identifier.</param>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 08-07-2022 created
        /// </Modified>
        public virtual void Remove(object id)
        {
            var entity = GetById(id);
            Remove(entity);
        }

        /// <summary>Xóa nhiều bản ghi</summary>
        /// <param name="entities">The entities.</param>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 08-07-2022 created
        /// </Modified>
        public virtual void RemoveMultiple(List<T> entities)
        {
            try
            {
                _context.Set<T>().RemoveRange(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError("RemoveMultiple" + ex.Message);
            }
        }

        /// <summary>Cập nhật thông tin</summary>
        /// <param name="entity">Đối tượng</param>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 6/27/2022 created
        /// </Modified>
        public virtual bool Update(T entity)
        {
            bool isSuccess = false;
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                _context.Set<T>().Update(entity);
                if (_context.SaveChanges() == 1) isSuccess = true;
                
            }
            catch (Exception ex)
            {

                _logger.LogError("Update" + ex.Message);
            }
            return isSuccess;
        }

        /// <summary>Cập nhật nhìu thông tin</summary>
        /// <param name="entities">Danh sách đối tượng</param>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 6/27/2022 created
        /// </Modified>
        public virtual void Updates(List<T> entities)
        {
            try
            {
                _context.Set<T>().UpdateRange(entities);

            }
            catch (Exception ex)
            {

                _logger.LogError("Updates" + ex.Message);
            }
        }

        /// <summary>Thêm hoặc sửa nhiều bản ghi</summary>
        /// <param name="entities">The entities.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 08-07-2022 created
        /// </Modified>
        public virtual void AddOrUpdateRange(List<T> entities, string columnName)
        {
            try
            {
                var set = _context.Set<T>();
                set.AddRange(entities);
            }
            catch (Exception ex)
            {

                _logger.LogError("AddOrUpdateRange" + ex.Message);
            }
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
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                }
            }
        }

        /// <summary>Lưu sự thay đổi của dữ liệu</summary>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 08-07-2022 created
        /// </Modified>
        public void SaveChange()
        {           
             _context.SaveChanges();     
        }
    }
}
