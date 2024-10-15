using Admin.Application.Interfaces;
using Admin.Application.ViewModels.Request;
using Admin.Application.ViewModels.Response;
using Admin.Domain.Entities;
using Base.Common.Event;
using Base.Common.Helper;
using Base.Common.Service.Interfaces;
using Base.Common.Services.Implementations;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Application.Services
{
    public class AdminUserService : BaseCRUDService<AdminUser, AdminUserRequestSearchModel, AdminUserViewModel, Guid>, IAdminUserService
    {
        public AdminUserService(IServiceProvider provider) : base(provider)
        {

        }

        public override async Task<HandleResult<GridBaseResponse<AdminUserViewModel>>> GetPage(AdminUserRequestSearchModel request)
        {
            var res = new HandleResult<GridBaseResponse<AdminUserViewModel>>() { Data = new(), Message = "Lấy dữ liệu không thành công" };

            try
            {
                var result = await _repository.GetListAsync();
                var dataMap = await MapDBData(result.ToList());
                res.Success = true;
                res.Data.Items = dataMap.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{GetType().Name}.{MethodHelper.GetNameAsync()}: {ex}");
            }
            return res;
        }

        public async Task<AdminUser> GetUserByUsername(string username)
        {
            AdminUser result = null;
            try
            {
                result = await _repository.GetSingleByConditionAsync(new { Username = username });
            }
            catch (Exception ex)
            {
                _logger.LogError($"{GetType().Name}.{MethodHelper.GetNameAsync()}: {ex}");
            }

            return result;
        }
    }
}
