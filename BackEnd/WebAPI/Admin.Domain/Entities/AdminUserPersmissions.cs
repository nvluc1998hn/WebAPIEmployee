using Base.Common.Attributes;
using Base.Domain.Models.EntityBase;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Domain.Entities
{
    public class AdminUserPersmissions : BaseModel<string>
    {
        [Key]
        [Column("FK_UserID", Order = 0)]
        public Guid FK_UserID { get; set; }

        [Key]
        [Column("FK_Permission", Order = 1)]
        public int FK_Permission { get; set; }

        [NotMapped]
        public override string Id => $"{FK_UserID}_{FK_Permission}";

        public bool? IsDeleted { get; set; }
    }
}
