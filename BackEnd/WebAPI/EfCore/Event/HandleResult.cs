using System.Collections.Generic;

namespace EmployeeManagement.Common.Event
{
    /// <summary> Dữ liệu trả về từ handle với kiểu chỉ định </summary>
    /// Author: trinhtx
    public class HandleResult<DataType>
    {
        /// <summary> Trạng thái khi thực hiện thành công </summary>
        public bool Success { get; set; }

        /// <summary> Dữ liệu trả về </summary>
        public DataType Data { get; set; }

        /// <summary> Thông báo trả về </summary>
        public string Message { get; set; }

        /// <summary> Thông báo lỗi nội bộ </summary>
        public string InternalMessage { get; set; }
    }

    /// <summary> Dữ liệu trả về từ handle với Data kiểu bất kỳ </summary>
    /// Author: trinhtx
    public class HandleResult : HandleResult<object>
    {
    }

    /// <summary> Dữ liệu trả về từ handle với Data là List </summary>
    /// Author: trinhtx
    public class HandleResultList<DataType> : HandleResult<List<DataType>>
    {
    }

    /// <summary> Dữ liệu trả về từ handle với Data là Dictionary </summary>
    /// Author: trinhtx
    public class HandleResultDictionary<KeyType, ValueType> : HandleResult<Dictionary<KeyType, ValueType>>
    {
    }
}