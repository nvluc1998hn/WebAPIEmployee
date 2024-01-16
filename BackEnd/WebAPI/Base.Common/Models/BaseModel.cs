using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Models
{
    public class BaseModel
    {

        public virtual DateTime CreatedDate { get; set; }

        public Guid? CreatedByUser { get; set; }

        public Guid? UpdatedByUser { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
