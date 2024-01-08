using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.EfCore.Event
{
    public class GridBaseResponse
    {
        /// <summary> Danh sách dữ liệu </summary>
        public List<object> Items { get; set; }

        /// <summary> Tổng số bản ghi </summary>
        public long TotalItems { get; set; }

        /// <summary> Dữ liệu Group tương ứng với các cột khai báo khi nhóm theo cột </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Dictionary<string, string>> GroupInfo { get; set; }

        /// <summary> Dữ liệu Total tương ứng với các cột khai báo khi nhóm theo cột </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> TotalInfo { get; set; }
    }

    /// <summary> Cấu trúc dữ liệu trả về client sau khi đã xử lý và phân trang </summary>
    public class GridBaseResponse<TResponse> : GridBaseResponse
    {
        public new List<TResponse> Items { get; set; }
    }
}
