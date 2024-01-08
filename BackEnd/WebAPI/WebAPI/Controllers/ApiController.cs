using EmployeeManagement.Common.Constant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace EmployeeManagementAPI.Controllers
{

    [Produces("application/json")]
    [Consumes("application/json")]
    public class ApiController : ControllerBase
    {
        private readonly ILogger<ApiController> _logger;

        public ApiController()
        {

        }

        public ApiController(ILogger<ApiController> logger)
        {
            _logger = logger;
        }

        protected bool IsValidOperation()
        {
            return true;
        }

        protected new ApiResponse Response(object result = null)
        {
            if (IsValidOperation())
            {
                return new ApiOkResultResponse(result);
            }

            return new ApiBadRequestResponse("Sai thông tin", "Có lỗi sảy ra khi thực hiện", "");
        }

        protected new IActionResult Response(object result = null, string usermessage = "Lấy dữ liệu thành công", string internalmessage = "Thành công")
        {
            return Ok(new ApiOkResultResponse(result, usermessage, internalmessage));
        }

    }
}
