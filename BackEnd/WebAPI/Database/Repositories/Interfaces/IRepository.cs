using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Database.Repositories.Interfaces
{
    /// <summary>
    ///   Thực hiện các thao tác thêm sửa xóa với DB
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Key">Khóa chính của bảng</typeparam>
    /// <Modified>
    /// Name Date Comments
    /// lucnv 11-07-2022 created
    /// </Modified>
    public interface IRepository<T,Key> where T : class
    {
        /// <summary>Hàm tìm kiếm trả về 1 bản ghi.</summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 11-07-2022 created
        /// </Modified>
        T FindSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>Tìm kiếm nhiều bản ghi</summary>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 11-07-2022 created
        /// </Modified>
        IQueryable<T> FindAll(params Expression<Func<T, object>>[] includeProperties);

        /// <summary>Tìm kiếm nhiều bản ghi</summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 11-07-2022 created
        /// </Modified>
        IQueryable<T> FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>Lấy nhiều bản ghi</summary>
        /// <param name="where">The where.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 11-07-2022 created
        /// </Modified>
        IQueryable<T> GetMany(Expression<Func<T, bool>> where);

        /// <summary>Thêm mới dữ liệu</summary>
        /// <param name="entity">The entity.</param>
        /// <param name="keyField">The key field.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 11-07-2022 created
        /// </Modified>
        Key Insert(T entity, string keyField);

        /// <summary>Cập nhật dữ liệu</summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 11-07-2022 created
        /// </Modified>
        bool Update(T entity);

        /// <summary>Cập nhật nhiều dữ liệu</summary>
        /// <param name="entities">The entities.</param>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 11-07-2022 created
        /// </Modified>
        void Updates(List<T> entities);

        /// <summary>Thêm mới nhiều dữ liệu</summary>
        /// <param name="entities">The entities.</param>
        /// <param name="idColumn">The identifier column.</param>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 11-07-2022 created
        /// </Modified>
        void AddOrUpdateRange(List<T> entities, string idColumn);

        /// <summary>Xóa dữ liệu</summary>
        /// <param name="entity">The entity.</param>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 11-07-2022 created
        /// </Modified>
        void Remove(T entity);

        /// <summary>Xóa dữ liệu theo ID</summary>
        /// <param name="id">The identifier.</param>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 11-07-2022 created
        /// </Modified>
        void Remove(object id);

        /// <summary>Lấy tất cả bản ghi không theo điều kiện</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 11-07-2022 created
        /// </Modified>
        IQueryable<T> GetAll();

        T Get(Expression<Func<T, bool>> where);

        /// <summary>Lấy dữ liệu bản ghi theo ID</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 11-07-2022 created
        /// </Modified>
        T GetById(object id);

        /// <summary>Xóa nhiều bản ghi</summary>
        /// <param name="entities">The entities.</param>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 11-07-2022 created
        /// </Modified>
        void RemoveMultiple(List<T> entities);

        /// <summary>Lưu</summary>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 11-07-2022 created
        /// </Modified>
        void SaveChange();
    }
}
