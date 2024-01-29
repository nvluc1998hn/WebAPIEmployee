using Base.Common.Event;
using Base.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Service.Interfaces
{
    public interface IBaseCRUDService<TRequest, TRquestSearch, TResponse, Id> : IGridBaseService<TRquestSearch, TResponse> where TRequest : BaseModel
    {
        Task<HandleResult> Add(TRequest data);

        Task<HandleResult> Update(TRequest data);

        Task<HandleResult> Delete(TRequest data);

    }
}
