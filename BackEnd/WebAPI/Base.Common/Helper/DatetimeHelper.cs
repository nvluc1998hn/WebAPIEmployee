using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Helper
{
    public static class DatetimeHelper
    {
        public static string ToSqlDatetime(this DateTime date, bool onlyDate = false)
        {
            var format = "yyyy-MM-dd" + (onlyDate ? "" : " HH:mm:ss");
            return date.ToString(format, CultureInfo.InvariantCulture);
        }
    }
}
