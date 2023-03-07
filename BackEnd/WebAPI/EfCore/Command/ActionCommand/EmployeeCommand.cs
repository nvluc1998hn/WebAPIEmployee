using EmployeeManagement.EfCore.Validations;
using FluentValidation;
using System;


namespace EmployeeManagement.Common.Command.ActionCommand
{
    public abstract class EmployeeCommand : HandleRequest
    {
        public Guid EmployeeID { get; set; }
        public string FullName { get; set; }
    }
}
