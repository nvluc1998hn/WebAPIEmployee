

using EmployeeManagement.Common.Command.ActionCommand;
using EmployeeManagement.EfCore.Validations;

namespace EmployeeManagement.EfCore.Command.ActionCommand
{
    public class InsertEmployeeCommand : EmployeeCommand
    {
        public override bool IsValid()
        {
            ValidationResult = new InsertEmployeeCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}

