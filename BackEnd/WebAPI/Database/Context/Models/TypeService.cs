using Base.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Domain.Models.EntityBase;


namespace EmployeeManagement.Database.Context.Models
{
    [Table("TypeService")]
    public class TypeService:BaseModel<Guid>
    {
        [Key]
        [NotNull]
        public Guid TypeServiceId { get; set; }

        public string TypeServiceName { get; set; }

        public double Price { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime DateApply { get; set; }

        [NotMapped]
        [Column("TypeServiceId")]
        public override Guid Id { get; set; }
    }
}
