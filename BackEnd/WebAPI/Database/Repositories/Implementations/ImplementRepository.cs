using EmployeeManagement.Database.Repositories.Interfaces;
using EmployeeManagement.Database.Context;
using EmployeeManagement.Database.Context.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Database.Repositories.Implementations
{
    public class EmployeeRepository : Repository<Employee, Guid>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext context, ILogger<Employee> logger) : base(context, logger)
        {
        }
       
    }

}
