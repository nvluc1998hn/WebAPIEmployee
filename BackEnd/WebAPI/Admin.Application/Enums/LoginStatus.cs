using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Application.Enums
{
    public enum LoginStatus : byte
    {
        None = 0,
        Success = 1,
        LoginFailed = 2,
        AccountLockedOrDeleted = 3,
        AccountNeedActive = 4,
        WrongUsernameOrPassword = 5,
        WrongUsernameOrPasswordNeedCaptcha = 6,
        WrongUsernameOrPasswordLocked = 7,
        CompanyNotExist = 8,
        CompanyLockedOrDeleted = 9,
        CannotCreateToken
    }
}
