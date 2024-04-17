using Admin.Application.Interfaces;
using Admin.Application.ViewModels.Request;
using Admin.Application.ViewModels.Response;
using Admin.Domain.Entities;
using EmployeeManagementAPI.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Admin.API.Controllers
{
    [Route("api/v1/admin")]
    [ApiVersion("1")]
    public class AdminUserController : GridBaseCRUDController<AdminUser, AdminUserRequestSearchModel, AdminUserViewModel, IAdminUserService>
    {
        public AdminUserController(IServiceProvider provider) : base(provider)
        {

        }
    }
}
