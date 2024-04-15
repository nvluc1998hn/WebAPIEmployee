using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Jwt.Models
{
    public class JwtOptions
    {
        public bool Enabled { get; set; }

        public string SecretKey { get; set; }

        public string Issuer { get; set; }

        public int ExpiredMinutes { get; set; }

        public bool ValidateLifetime { get; set; }

        public bool ValidateAudience { get; set; }

        public string ValidAudience { get; set; }

        public string AuthenUri { get; set; }

        public List<string> ExceptPattern { get; set; }
    }
}
