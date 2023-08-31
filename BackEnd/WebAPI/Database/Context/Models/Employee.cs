using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using EmployeeManagement.Common.Attributes;

namespace EmployeeManagement.Database.Context.Models
{
    [Table("Employee")]
    public partial class Employee
    {
        [Key]
        [NotNull]
        public Guid EmployeeID { get; set; }

        [NotNull]
        public string FullName { get; set; }

        [NotNull]
        public DateTime? DateOfBirth { get; set; }
              
        public string Email { get; set; }

        [NotNull]
        public string Phone { get; set; }

        [NotNull]
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
