﻿using Base.Common.Attributes;
using Base.Domain.Models.EntityBase;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Admin.Domain.Entities
{
    public class AdminStaff : BaseModel<int>
    {
        [Key]
        [Column("StaffId")]
        [IgnoreInsert]
        public override int Id { get; set; }

        public string StaffCode { get; set; }

        public string StaffName { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Image { get; set; }

        public int Part { get; set; }

        public int Sex { get; set; }

        public bool IsLock { get; set; }

        public bool IsDelete { get; set; }
    }
}
