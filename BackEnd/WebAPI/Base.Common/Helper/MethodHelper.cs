using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Helper
{
    public static class MethodHelper
    {
        /// <summary> Lấy tên của method có async </summary>
        public static string GetNameAsync([CallerMemberName] string name = "") => name;
    }
}
