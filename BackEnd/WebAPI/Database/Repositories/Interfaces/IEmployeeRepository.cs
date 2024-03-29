﻿using EmployeeManagement.Database.Context.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Database.Repositories.Interfaces
{
    public interface IEmployeeRepository : IRepository<Employee, Guid>
    {
        
    }
    
}
