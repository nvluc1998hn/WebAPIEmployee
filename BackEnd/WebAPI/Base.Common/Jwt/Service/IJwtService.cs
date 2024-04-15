using Base.Common.Jwt.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Jwt.Service
{
    public interface IJwtService
    {
        /// <summary> Tạo token </summary>
        JsonWebToken CreateToken(Guid userId, int xnCode, string customerCode = null, Guid? loginUserId = null);

        /// <summary> Xác thực token </summary>
        /// <param name="context"> Context thực hiện. Nếu không truyền sẽ lấy trong Context hiện tại </param>
        Task<bool> ValidateToken(HttpContext context = null);

        Task<bool> IsCurrentActiveToken();

        Task DeactivateCurrentAsync();

        Task<bool> IsActiveAsync(string token);

        Task DeactivateAsync(string token);

        /// <summary> Lấy ra TokenPayload </summary>
        JsonWebTokenPayload GetTokenPayload(string accessToken);
    }
}
