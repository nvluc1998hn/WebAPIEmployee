namespace Base.Common.Models
{
    /// <summary>
    /// Model chứa param tìm kiếm dữ liệu chung
    /// </summary>
    /// <Modified>
    /// Name Date Comments
    /// lucnv 16/04/2024 created
    /// </Modified>
    public class BaseRequestModel : BaseRequest
    {
        public string? KeyWordSearch { get; set; }

        public DateTime FromDate { get; set; }

        public int FK_CompanyId { get; set; }

        public DateTime ToDate { get; set; }



    }
}
