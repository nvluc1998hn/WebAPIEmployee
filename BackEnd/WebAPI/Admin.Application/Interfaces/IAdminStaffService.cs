using Admin.Domain.Entities;
using Base.Common.Service.Interfaces;

namespace Admin.Application.Interfaces
{
    public interface IAdminStaffService : IBaseService<AdminStaff, int>
    {
        public Task<IEnumerable<AdminStaff>> GetListStaffByCondition(string keyword);
    }
}
