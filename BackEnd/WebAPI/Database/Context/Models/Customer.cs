using Base.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Base.Domain.Models.EntityBase;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Database.Context.Models
{
    [Table("Customer")]
    public class Customer : BaseModel<Guid>
    {
        [Key]
        [NotNull]
        public Guid CustomerId { get; set; }

        [NotNull]
        public string FullName { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public bool? IsDeleted { get; set; }


        [NotNull]
        public DateTime DateOfBirth { get; set; }

        [NotMapped]
        [Column("CustomerId")]
        public override Guid Id { get; set; }
    }
}
