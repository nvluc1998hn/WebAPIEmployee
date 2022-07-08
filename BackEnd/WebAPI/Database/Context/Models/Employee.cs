using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EmployeeManagement.Database.Context.Models
{
    public partial class Employee
    {
        [Key]
        [NotNull]
        public Guid EmployeeID { get; set; }

        [NotNull]
        [StringLength(150)]
        public string FullName { get; set; }

        [NotNull]
        public DateTime? DateOfBirth { get; set; }
              
        public string Email { get; set; }

        [NotNull]
        [StringLength(20)]
        public string Phone { get; set; }

        [NotNull]
        [StringLength(500)]
        public string PassWord { get; set; }

        [NotNull]
        public byte Sex { get; set; }

        [NotNull]
        public bool IsDelete { get; set; }

        [NotNull]
        public DateTime CreateDate { get; set; }

        [NotNull]
        public Guid CreatorAdd { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public Guid? CreatorEdit { get; set; }
     


    }
}
