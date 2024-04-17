using Base.Domain.Models.EntityBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Domain.Entities
{
    public class AdminUser: BaseModel<Guid>
    {
        [Key]
        [Column("PK_UserID")]
        public Guid PK_UserID { get; set; }
        public int FK_CompanyID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Fullname { get; set; }

        [Base.Common.Attributes.IgnoreUpdate]
        public Guid CreatedByUser { get; set; }

        [Base.Common.Attributes.IgnoreUpdate]
        public  DateTime CreatedDate { get; set; }

        [Base.Common.Attributes.IgnoreInsert]
        public Guid? UpdatedByUser { get; set; }

        [Base.Common.Attributes.IgnoreInsert]
        public  DateTime? UpdatedDate { get; set; }



    }

}
