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
    [Table("Vehicle")]
    public class Vehicle
    {
        [Key]
        [NotNull]
        public Guid VehicleID { get; set; }

        public Guid FK_VehicleGroupID { get; set; }

        public string VehiclePlate { get; set; }

        public string PrivateCode { get; set; }

        bool IsDeleted { get; set; }

        [NotNull]
        public DateTime CreatedDate { get; set; }

        [NotNull]
        public Guid CreatedByUser { get; set; }

        public DateTime? UpdatedByUser { get; set; }

        public Guid? UpdatedDate { get; set; }

    }
}
