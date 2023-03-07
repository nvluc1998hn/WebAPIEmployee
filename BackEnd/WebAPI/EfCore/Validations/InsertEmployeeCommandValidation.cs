using EmployeeManagement.Common.Command.ActionCommand;
using EmployeeManagement.Database.Context.Models;
using EmployeeManagement.EfCore.Command.ActionCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.EfCore.Validations
{
    public class InsertEmployeeCommandValidation : EmployeeCommandValidation<InsertEmployeeCommand>
    {
        public InsertEmployeeCommandValidation() {
         //   ValidatePk();
            ValidateFullName();
        }
    }
}
