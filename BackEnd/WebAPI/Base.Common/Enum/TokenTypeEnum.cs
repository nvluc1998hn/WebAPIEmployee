using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Enum
{
    public enum TokenTypeEnum : byte
    {
        Basic = 1,
        Bearer = 2
    }

    /// <summary>
    /// Thời gian timeout khi gọi Service
    /// </summary>
    public enum HttpClientTimeOutEnum
    {
        /// <summary>Thời gian tăng lên 15s để ko bị timeout lệch với Polly.</summary>
        TimeOut = 15000
    }

}
