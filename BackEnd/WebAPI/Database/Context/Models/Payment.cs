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
    /// Bảng chi tiền
    /// </summary>
    /// <Modified>
    /// Name Date Comments
    /// lucnv 23/01/2024 created
    /// </Modified>
    /// <seealso cref="Base.Common.Models.BaseModel" />
    [Table("Payment")]
    public class Payment : BaseModel<Guid>
    {
        [Key]
        [NotNull]
        public Guid PaymentId { get; set; }

        public double Amount { get; set; }

        public string Description { get;set; }

        public DateTime DatePayment { get; set; }

        [NotMapped]
        [Column("PaymentId")]
        public override Guid Id { get; set; }
    }
}
