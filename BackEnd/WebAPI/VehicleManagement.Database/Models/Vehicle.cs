using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleManagement.Database.Models
{
    [Table("Vehicle")]
    public class Vehicle
    {
        [Key]
        [NotNull]
        public Guid VehicleID { get; set; }

        public Guid? FK_VehicleGroupID { get; set; }

        public string VehiclePlate { get; set; }

        public string PrivateCode { get; set; }

        public bool? IsDeleted { get; set; }

        public Guid? CreatedByUser { get; set; }

        public DateTime? CreatedDate { get; set; }

        public Guid? UpdatedByUser { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
