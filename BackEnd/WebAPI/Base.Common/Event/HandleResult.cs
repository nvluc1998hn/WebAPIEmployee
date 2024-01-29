using System.Collections.Generic;

namespace Base.Common.Event
{
    /// <summary> Dữ liệu trả về từ handle với kiểu chỉ định </summary>
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
    public class HandleResult : HandleResult<object>
    {
    }

    /// <summary> Dữ liệu trả về từ handle với Data là List </summary>
    public class HandleResultList<DataType> : HandleResult<List<DataType>>
    {
    }

    /// <summary> Dữ liệu trả về từ handle với Data là Dictionary </summary>
    public class HandleResultDictionary<KeyType, ValueType> : HandleResult<Dictionary<KeyType, ValueType>>
    {
    }
}