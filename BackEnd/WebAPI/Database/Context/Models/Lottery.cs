using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Database.Context.Models
{
    [Table("Lottery")]

    public class Lottery
    {
        [Key]
        [NotNull]
        public Guid LotteryID { get; set; }

        public string NumberLottery { get; set; }

        public int? Amount { get; set; }

        public int? TypeLottery { get; set; }

        public DateTime CreatedDate { get; set; }

        public Guid? CreatedByUser { get; set; }

        public Guid? UpdatedByUser { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
