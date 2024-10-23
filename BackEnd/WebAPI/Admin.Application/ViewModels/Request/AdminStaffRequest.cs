
using Base.Common.Attributes;
using Base.Common.Validation;

namespace Admin.Application.ViewModels.Request
{
    public class AdminStaffRequest : BaseValidatorMessage
    {
        public int Id { get; set; }

        [ValidStringAttribute]
        public string StaffCode { get; set; }

        [ValidStringAttribute]
        public string StaffName { get; set; }

        [ValidStringAttribute]
        public string Address { get; set; }

        [ValidStringAttribute]
        public string Phone { get; set; }

        [ValidEmailAttribute]
        public string Email { get; set; }

        public string Image { get; set; }

        public int Part { get; set; }

        public int Sex { get; set; }

        public Guid CreateByUser { get; set; }

        public Guid UpdateByUser { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new AdminStaffRequestValid().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
