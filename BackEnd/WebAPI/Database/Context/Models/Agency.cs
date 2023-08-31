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
    [Table("Agency")]
    public class Agency
    {
        [Key]
        [NotNull]
        public Guid AgencyId { get; set; }

        public string NameAgency { get; set; }

        public int PriceAmountLot { get; set; }

        public int PaymentAmoutLot { get; set; }

        public int PaymentAmountLopic { get; set; }

        public DateTime CreatedDate { get; set; }

        public Guid? CreatedByUser { get; set; }

        public Guid? UpdatedByUser { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
