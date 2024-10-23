using Admin.Application.ViewModels.Request;
using Admin.Domain.Entities;
using Base.Common.Service.Interfaces;

namespace Admin.Application.Interfaces
{
    /// <summary>
    ///  Thông tin bảng nhân viên
    /// </summary>
    /// Author: lucnv
    /// Created: 17/10/2024
    public interface IAdminStaffService : IBaseService<AdminStaff, int>
    {
        public Task<IEnumerable<AdminStaff>> GetListStaffByCondition(string keyword);

        public Task<bool> UpdateData(AdminStaffRequest adminStaff);

        public Task<bool> InsertData(AdminStaffRequest adminStaff);

        public Task<bool> DeleteData(int adminId);
    }
}
