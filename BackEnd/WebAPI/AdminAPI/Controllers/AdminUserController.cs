using Admin.Application.Interfaces;
using Admin.Application.ViewModels.Request;
using Admin.Application.ViewModels.Response;
using Admin.Domain.Entities;
using Base.Common.Grid;
using EmployeeManagementAPI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Helpers;

namespace Admin.API.Controllers
{
    [Route("api/v1/admin")]
    [ApiVersion("1")]
    public class AdminUserController : GridBaseCRUDController<AdminUser, AdminUserRequestSearchModel, AdminUserViewModel, IAdminUserService>
    {
        public AdminUserController(IServiceProvider provider) : base(provider)
        {

        }

        protected override GridPermissions Permissions => new()
        {

        };

        public override AdminUser ProcessRequest(AdminUser request)
        {
            request.Password = StringHelper.EncryptPassword(request.Password);
            return request;
        }
    }
}
