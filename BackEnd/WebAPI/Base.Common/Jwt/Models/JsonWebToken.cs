using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Jwt.Models
{
    public class JsonWebToken
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public DateTime Expires { get; set; }

        [JsonProperty("userId")]
        public string Id { get; set; }

        public IDictionary<string, string> Claims { get; set; }
    }
}
