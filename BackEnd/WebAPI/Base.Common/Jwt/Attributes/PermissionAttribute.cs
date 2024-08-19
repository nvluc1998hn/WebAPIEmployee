using Base.Common.Jwt.Claims;
using Base.Common.Respone;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Jwt.Attributes
{
    public class PermissionAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        // Kiểu thực hiện API
        public int[] Permissions { get; set; }

        public int[] PermissionsContain { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var permission = context.HttpContext.HasPermission(Permissions, PermissionsContain);

            // Validate if any permissions are passed when using attribute at controller or action level
            if (!permission)
            {
                // Không có quyền
                context.Result = new ApiForbidResponse();
            }
        }
    }
}
