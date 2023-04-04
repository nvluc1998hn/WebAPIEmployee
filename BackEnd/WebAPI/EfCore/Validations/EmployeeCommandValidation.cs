using EmployeeManagement.Common.Command.ActionCommand;
using EmployeeManagement.Database.Context.Models;
using EmployeeManagement.EfCore.Command.ActionCommand;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.EfCore.Validations
{
    public class EmployeeCommandValidation<T> : AbstractValidator<T> where T : EmployeeCommand
    {

        protected void ValidatePk()
        {
            RuleFor(c => c.EmployeeID)
                .NotEmpty().WithMessage("Employee không được bỏ trống ID");
        }
        protected void ValidateFullName()
        {
            RuleFor(c => c.FullName)
                .NotEmpty().WithMessage("Tên nhân viên không được bỏ trống !");
        }
    }
}
