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
    [Table("AdminUsersLogin")]
    public class AdminUser: BaseModel<Guid>
    {
        [Key]
        [NotMapped]
        [Column("PK_UserID")]
        public override Guid Id { get; set; }

        public int FK_CompanyID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Fullname { get; set; }
    }

}
