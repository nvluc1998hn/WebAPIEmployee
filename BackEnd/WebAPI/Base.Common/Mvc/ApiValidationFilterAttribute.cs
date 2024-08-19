using Base.Common.Constant;
using Base.Common.Enum;
using Base.Common.LanguageKeys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Mvc
{
    public class ApiValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var lsterror = context.ModelState.SelectMany(x => x.Value.Errors).Select(x => x.ErrorMessage).FirstOrDefault().Split("|");

                if (lsterror.Length > 1)
                {
                    context.Result = new BadRequestObjectResult(new ApiBadRequestResponse(ResponseCodeEnums.ParamsInvalid, lsterror[0].Trim(), lsterror[1].Trim()));
                }
                else
                {
                    context.Result = new BadRequestObjectResult(new ApiBadRequestResponse(ResponseCodeEnums.ParamsInvalid, "ErrorParamsInvalid", lsterror[0].Trim()));
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
