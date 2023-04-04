using EmployeeManagement.Database.Repositories.Interfaces;
using EmployeeManagement.Database.Context;
using EmployeeManagement.Database.Context.Models;
using System;
using Microsoft.Extensions.Logging;
using EmployeeManagement.Database.Repositories.Implementations;

namespace EmployeeManagement.Database.Repositories.EmployeeRepository
{
    public class EmployeeRepository : Repository<Employee, Guid>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext context, ILogger<Employee> logger) : base(context, logger)
        {
        }
       
    }

}
