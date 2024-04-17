using Admin.Application.ViewModels.Request;
using Admin.Application.ViewModels.Respond;
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
        Task<LoginViewModel> CreateLoginSession(Guid userId, Guid? loginUserId = null);

        /// <summary> Tạo token </summary>
        string CreateAccessToken(Guid userId, int xnCode, string customerCode = null, Guid? loginUserId = null);
    }
}
