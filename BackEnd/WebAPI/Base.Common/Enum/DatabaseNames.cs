﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Enum
{
    /// <summary> Danh sách database trong hệ thống </summary>
    /// <Modified> Name Date Comments trungtq 4/9/2013 created </Modified>
    public enum DatabaseNames
    {
        /// <summary> Database server mặc định </summary>
        Default,

        /// <summary> Database server 1 </summary>
        DatabaseServer1,

        /// <summary> Database server 2 </summary>
        DatabaseServer2,

        /// <summary> Database server 3 </summary>
        DatabaseServer3,

        /// <summary> Database server 4 Hiện tại đang dùng cho partner (CNS, GISVIET) </summary>
        DatabaseServer4,

        /// <summary> Database server 5 </summary>
        DatabaseServer5,

        /// <summary> Database server 6 </summary>
        DatabaseServer6,

        /// <summary> Database server 100 </summary>
        DatabaseServer100 = 100
    }
}
