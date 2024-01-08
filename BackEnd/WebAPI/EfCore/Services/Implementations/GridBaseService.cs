using EmployeeManagement.Common.Event;
using EmployeeManagement.EfCore.Event;
using EmployeeManagement.EfCore.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.EfCore.Services.Implementations
{
    public class GridBaseService<TRequest, TResponse> : IGridBaseService<TRequest, TResponse>
    {

        public Task<HandleResult<GridBaseResponse<TResponse>>> GetPage(TRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
