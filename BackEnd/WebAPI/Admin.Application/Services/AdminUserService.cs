using Admin.Application.Interfaces;
using Admin.Application.ViewModels.Request;
using Admin.Application.ViewModels.Response;
using Admin.Domain.Entities;
using Base.Common.Service.Interfaces;
using Base.Common.Services.Implementations;
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
    }
}
