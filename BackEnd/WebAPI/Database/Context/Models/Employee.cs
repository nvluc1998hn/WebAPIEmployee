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
        public DateTime? DateOfBirth { get; set; }
        [NotNull]
        [StringLength(250)]
        public string Email { get; set; }
        [StringLength(20)]
        public string Phone { get; set; }
        [NotNull]
        [StringLength(500)]
        public string PassWord { get; set; }
        public byte Sex { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatorAdd { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string CreatorEdit { get; set; }


    }
}
