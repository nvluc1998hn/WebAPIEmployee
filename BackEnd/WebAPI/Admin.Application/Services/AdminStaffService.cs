using Admin.Application.Interfaces;
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
    }
}
