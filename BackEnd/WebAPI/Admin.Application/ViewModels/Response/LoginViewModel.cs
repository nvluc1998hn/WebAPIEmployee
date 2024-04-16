using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Application.ViewModels.Respond
{
    public class LoginViewModel
    {
        [JsonProperty("1")]
        public int CompanyId { set; get; }

        [JsonProperty("2")]
        public Guid UserId { set; get; }

        [JsonProperty("3")]
        public string UserName { get; set; }

        [JsonProperty("4")]
        public string FullName { get; set; }

        [JsonProperty("5")]
        public List<int> Permissions { set; get; }
    }
}
