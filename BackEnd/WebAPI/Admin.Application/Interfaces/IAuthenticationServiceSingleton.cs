using Admin.Application.ViewModels.Request;
using Admin.Application.ViewModels.Respond;
using Admin.Domain.Entities;
using Base.Common.Jwt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Application.Interfaces
{
    public interface IAuthenticationServiceSingleton
    {
        Task<LoginViewModel> Login(LoginRequest request);

        /// <summary> Tạo phiên đăng nhập mới theo user đã xác thực </summary>
        Task<LoginViewModel> CreateLoginSession(AdminUser adminUser, LoginViewModel loginInfo = null);

        /// <summary> Lấy thông tin user có trong token </summary>
        Task<Dictionary<Guid, UserAuthenModel>> GetUserByToken(AuthenRequest request);

        /// <summary> Tạo token </summary>
        string CreateAccessToken(Guid userId);
    }
}
