using EmployeeManagement.Database.Context.Models;
using EmployeeManagement.EfCore.ViewModels.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.EfCore.Services.Interfaces
{
    /// <summary>Loại dịch vụ</summary>
    /// <Modified>
    /// Name Date Comments
    /// lucnv 06/01/2024 created
    /// </Modified>
    public interface ITypeServiceService:IBaseCRUDService<TypeService, TypeServiceRequestSearch, TypeService, Guid>
    {
        
        public Task<Dictionary<Guid, TypeService>> GetListTypeServiceById(List<Guid> Ids);
    }
}
