using Admin.Application.ViewModels.Request;
using Admin.Application.ViewModels.Response;
using Admin.Domain.Entities;
using Base.Common.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Application.Interfaces
{
    /// <summary>
    /// Thông tin người dùng
    /// </summary>
    /// <Modified>
    /// Name Date Comments
    /// lucnv 08/08/2024 created
    public interface IAdminUserService : IBaseCRUDService<AdminUser, AdminUserRequestSearchModel, AdminUserViewModel, Guid>
    {
        Task<AdminUser> GetUserByUsername(string username);

    }
}
