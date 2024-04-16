using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Enum
{
    public enum ResponseCodeEnums
    {
        [Description("Thành công")]
        Success = 0,

        [Description("Tham số đầu vào không hợp lệ")]
        ParamsInvalid = 1,

        [Description("Dịch vụ đang tạm dừng")]
        ServiceStop = 2,

        [Description("Sai thông tin key")]
        ErrorKey,

        [Description("Địa chỉ IP không có quyền truy xuất")]
        IpUnAuthorize,

        [Description("Không có quyền sử dụng phương thức")]
        UnAuthorize,

        [Description("Các lỗi khác")]
        Other,

        [Description("Đã tồn tại")]
        AlreadyExist,

        [Description("Lỗi hệ thống")]
        CatchError
    }
}
