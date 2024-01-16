using Base.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Database.Context.Models
{
    /// <summary>
    /// Quan hệ khách hàng - dịch vụ
    /// </summary>
    /// <Modified>
    /// Name Date Comments
    /// lucnv 09/01/2024 created
    /// </Modified>
    [Table("CustomerService")]
    public class CustomerService2:BaseModel
    {
        [Key]
        [Column("CustomerId", Order = 0)]
        public Guid CustomerId { get; set; }


        [Key]
        [Column("TypeServiceId", Order = 1)]
        public Guid TypeServiceId { get; set; }

        [Key]
        [Column("InvoiceDate", Order = 2)]
        public DateTime InvoiceDate { get; set; }
    }
}
