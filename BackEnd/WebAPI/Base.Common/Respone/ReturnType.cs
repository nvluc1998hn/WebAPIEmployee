using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Respone
{
    public class ReturnType
    {
        public bool Status { get; set; }

        public object Value { get; set; }

        public string ValueType { get; set; }

        public string Description { get; set; }
    }
}
