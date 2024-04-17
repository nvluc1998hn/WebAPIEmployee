using Base.Common.Event;
using Base.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Service.Interfaces
{
    /// <summary>
    ///   <para>CRUD Base</para>
    /// </summary>
    /// <typeparam name="TRequest">Model gốc</typeparam>
    /// <typeparam name="TRquestSearch">Model tìm kiếm</typeparam>
    /// <typeparam name="TResponse">Kết quả trả về</typeparam>
    /// <typeparam name="Id">Kiểu của khóa chính bảng</typeparam>
    /// <Modified>
    /// Name Date Comments
    /// lucnv 17/04/2024 created
    /// </Modified>
    public interface IBaseCRUDService<TRequest, TRquestSearch, TResponse, Id> : IGridBaseService<TRquestSearch, TResponse> 
    {
        Task<HandleResult> Add(TRequest data);

        Task<HandleResult> Update(TRequest data);

        Task<HandleResult> Delete(TRequest data);

    }
}
