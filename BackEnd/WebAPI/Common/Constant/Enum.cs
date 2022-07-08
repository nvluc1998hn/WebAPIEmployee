using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Common.Constant
{
    public static class Enum
    {

        /// <summary>
        ///   Các thông báo cho client 
        /// </summary>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 04-07-2022 created
        /// </Modified>
        public enum messageCode
        {
            LoginFailed, // Tài khoản mật khẩu sai 
            UpdateRequired, 
            Locked, // Tài khoản bị khóa 
            WrongAppType, // Sai kiểu
        }
        public enum StatusCode
        {
            LoginSucces = 0,
            LoginFalse = 1,
            Success = 200,
            False = 500
            
        }
       

    }
}
