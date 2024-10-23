using Admin.Application.Interfaces;
using Admin.Application.ViewModels.Request;
using Admin.Domain.Entities;
using Base.Common.Helper;
using Base.Common.Services.Implementations;
using Microsoft.Extensions.Logging;

namespace Admin.Application.Services
{
    public class AdminStaffService : GridBaseService<AdminStaff, int>, IAdminStaffService
    {
        public AdminStaffService(IServiceProvider provider) : base(provider)
        {

        }

        public async Task<IEnumerable<AdminStaff>> GetListStaffByCondition(string keyword)
        {
            IEnumerable<AdminStaff> result = null;
            try
            {
                result = await _repository.GetListAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError($"{GetType().Name}.{MethodHelper.GetNameAsync()}: {ex}");
            }

            return result;
        }

        public async Task<bool> InsertData(AdminStaffRequest adminStaff)
        {
            bool isInsertSuccess = false;
            try
            {
                var dataInsert = new AdminStaff();
                dataInsert.StaffName = adminStaff.StaffName;
                dataInsert.Address = adminStaff.Address;
                dataInsert.StaffCode = adminStaff.StaffCode;
                dataInsert.Phone = adminStaff.Phone;
                dataInsert.Email = adminStaff.Email;
                dataInsert.Part = adminStaff.Part;
                dataInsert.Sex = adminStaff.Sex;
                dataInsert.Image = adminStaff.Image;
                dataInsert.CreatedDate = DateTime.Now;
                dataInsert.CreatedByUser = adminStaff.CreateByUser;
                var result = await InsertAsync(dataInsert);
                if (!string.IsNullOrEmpty(result?.StaffName))
                {
                    isInsertSuccess = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{GetType().Name}.{MethodHelper.GetNameAsync()}: {ex}");
            }
            return isInsertSuccess;
        }


        public async Task<bool> DeleteData(int adminId)
        {
            bool isDeleteSuccess = false;

            try
            {
                var dataInDb = _repository.GetById(adminId);

                var result = await _repository.DeleteAsync(dataInDb);

                if (result?.Id > 0)
                {
                    isDeleteSuccess = true;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"{GetType().Name}.{MethodHelper.GetNameAsync()}: {ex}");
            }
            return isDeleteSuccess;
        }

        public async Task<bool> UpdateData(AdminStaffRequest adminStaff)
        {
            bool isSuccess = false;
            try
            {
                var dataInDb = _repository.GetById(adminStaff.Id);
                dataInDb.StaffName = adminStaff.StaffName;
                dataInDb.Address = adminStaff.Address;
                dataInDb.Phone = adminStaff.Phone;
                dataInDb.Email = adminStaff.Email;
                dataInDb.Image = adminStaff.Image;
                dataInDb.Part = adminStaff.Part;
                dataInDb.Sex = adminStaff.Sex;
                dataInDb.UpdatedDate = DateTime.Now;
                dataInDb.UpdatedByUser = adminStaff.CreateByUser;
                isSuccess = await UpdateAsync(dataInDb);
            }

            catch (Exception ex)
            {
                _logger.LogError($"{GetType().Name}.{MethodHelper.GetNameAsync()}: {ex}");
            }
            return isSuccess;
        }
    }
}
