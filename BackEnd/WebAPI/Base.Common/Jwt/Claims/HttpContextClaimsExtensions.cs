using Base.Common.Helper;
using Base.Common.Jwt.Models;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Jwt.Claims
{
    public static class HttpContextClaimsExtensions
    {
        public static Guid GetUserID(this HttpContext httpContext)
        {
            try
            {
                var strGuid = httpContext?.User?.Claims
                    .Where(x => x.Type == JwtRegisteredClaimNames.Sub)
                    .Select(x => x.Value)
                    .FirstOrDefault();
                return new Guid(strGuid);
            }
            catch
            {
                return Guid.Empty;
            }
        }
        public static Guid GetLoginUserID(this HttpContext httpContext)
        {
            try
            {
                var claim = httpContext?.User?.Claims
                    .FirstOrDefault(x => x.Type == JwtClaimsTypes.LoginUserId);
                if (claim != null)
                {
                    return new Guid(claim.Value);
                }
                else
                {
                    return GetUserID(httpContext);
                }
            }
            catch
            {
                return Guid.Empty;
            }
        }

        public static string GetUserName(this HttpContext httpContext)
        {
            var user = GetUserAuthen(httpContext);
            return user?.UserName;
        }

        public static UserAuthenModel GetLoginUserAuthen(this HttpContext httpContext)
        {
            UserAuthenModel user;
            try
            {
                var userId = GetLoginUserID(httpContext);
                if (httpContext.Items.TryGetValue(userId, out var userData))
                {
                    user = (UserAuthenModel)userData;
                }
                else
                {
                    user = null;
                }
            }
            catch (Exception ex)
            {
                user = null;

                Log.Fatal($"{MethodHelper.GetNameAsync()} has an exception: {ex}");
            }
            return user;
        }

        public static List<int> GetLoginPermissions(this HttpContext httpContext)
        {
            var user = GetLoginUserAuthen(httpContext);
            return user?.Permissions;
        }

        public static UserAuthenModel GetUserAuthen(this HttpContext httpContext)
        {
            UserAuthenModel user;
            try
            {
                var userId = httpContext.GetUserID();
                if (httpContext.Items.TryGetValue(userId, out var userData))
                {
                    user = (UserAuthenModel)userData;
                }
                else
                {
                    user = null;
                }
            }
            catch
            {
                user = null;
            }
            return user;
        }

        public static bool HasPermission(this HttpContext httpContext, int[] listPermission, int[] listPermisionContain = null)
        {
            bool result = false;

            if (listPermission?.Length == 1 && listPermission?[0] == 0)
            {
                result = true;
            }
            else
            {
                var permission = GetLoginPermissions(httpContext);

                if (permission != null)
                {
                    if (listPermisionContain?.Length > 0 && listPermisionContain.Any(c => permission.Contains(c)))
                    {
                        result = true;
                    }
                    else
                    {
                        // Trường hợp User cấp cứu
                        if (permission.Contains(-1))
                        {
                            result = true;
                        }
                        // User được phép thực thi API nếu có đủ các quyền được yêu cầu
                        else if (listPermission?.Length > 0 && permission?.Count > 0 && listPermission.All(x => permission.Contains(x)))
                        {
                            // User đủ quyền thực hiện yêu cầu
                            result = true;
                        }
                    }
                }
            }

            return result;
        }

    }
}
