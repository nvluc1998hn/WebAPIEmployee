using System.ComponentModel.DataAnnotations;

namespace Base.Domain.Models.EntityBase
{
    public abstract class BaseModel<TypeKey>
    {
        protected BaseModel()
        {
            CreatedDate = DateTime.Now;
        }

        public Guid CreatedByUser { get; set; }

        public DateTime CreatedDate { get; set; }

        public Guid? UpdatedByUser { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [Key] public virtual TypeKey Id { get; set; }

    }
}
