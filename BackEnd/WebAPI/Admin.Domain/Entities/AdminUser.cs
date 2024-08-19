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
    [Table("AdminUser")]
    public class AdminUser: BaseModel<Guid>
    {
        [Key]
        [Column("PK_UserID")]
        public override Guid Id { get; set; }

        public int FK_Agency { get; set; }

        public string Username { get; set; }

        public byte UserType { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public string Fullname { get; set; }

        public bool IsLock { get; set; }

    }

}
