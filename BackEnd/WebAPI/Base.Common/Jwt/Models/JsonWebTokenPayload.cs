using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Jwt.Models
{
    public class JsonWebTokenPayload
    {
        public string Subject { get; set; }

        public DateTime Expires { get; set; }

        public IDictionary<string, string> Claims { get; set; }
    }
}
