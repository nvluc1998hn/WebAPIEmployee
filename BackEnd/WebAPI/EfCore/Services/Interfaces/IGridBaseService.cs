using EmployeeManagement.Common.Event;
using EmployeeManagement.EfCore.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.EfCore.Services.Interfaces
{
    /// <summary>
    ///  Chứa những hàm lấy dữ liệu
    /// </summary>
    /// <Modified>
    /// Name Date Comments
    /// lucnv 06/01/2024 created
    /// </Modified>
    public interface IGridBaseService<TRquestSearch, TResponse>
    {
        /// <summary> Lấy dữ liệu grid phân trang </summary>
        Task<HandleResult<GridBaseResponse<TResponse>>> GetPage(TRquestSearch request);

    }
}
