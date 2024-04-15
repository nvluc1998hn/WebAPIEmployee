using Base.Common.Jwt.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Jwt
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

        public static UserAuthenModel GetUserAuthen(this HttpContext httpContext)
        {
            UserAuthenModel user;
            try
            {
                var userId = GetUserID(httpContext);
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


    }
}
