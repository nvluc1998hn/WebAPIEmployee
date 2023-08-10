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
    [Table("LotteryPrice")]
    public class LotteryPrice
    {
        [Key]
        [NotNull]
        public Guid LotteryPriceID { get; set; }

        public double Price { get; set; }

        public Guid BrandId { get; set; }

        public DateTime CreatedDate { get; set; }

        public Guid? CreatedByUser { get; set; }

        public Guid? UpdatedByUser { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
