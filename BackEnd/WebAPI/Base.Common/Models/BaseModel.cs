using Base.Common.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Models
{
    public abstract class BaseModel<TypeKey>
    {
        protected BaseModel()
        {
            CreatedDate = DateTime.Now;
        }

        public DateTime CreatedDate { get; set; }

        public Guid? CreatedByUser { get; set; }

        public Guid? UpdatedByUser { get; set; }

        public DateTime? UpdatedDate { get; set; } 

        [Key] public virtual TypeKey Id { get; set; }

    }
}
