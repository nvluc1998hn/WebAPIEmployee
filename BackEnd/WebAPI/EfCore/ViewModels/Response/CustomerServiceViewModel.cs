using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.EfCore.ViewModels.Response
{
    public class CustomerServiceViewModel
    {
        public Guid CustomerId { get; set;}

        // Tên khách hàng
        public string CustomerName { get; set;}

        public Guid TypeServiceId { get; set; }

        public string TypeServiceName { get; set; }

        /// <summary>
        /// Giá dịch vụ
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 09/01/2024 created
        /// </Modified>
        public double Price { get; set; }

        /// <summary>
        /// Ngày tạo phiếu
        /// </summary>
        /// <value>
        /// The invoice date.
        /// </value>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 09/01/2024 created
        /// </Modified>
        public DateTime InvoiceDate { get; set; }

    }
}
