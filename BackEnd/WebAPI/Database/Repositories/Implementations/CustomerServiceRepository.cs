using Base.Common.Dapper;
using Base.Common.Implementations;
using EmployeeManagement.Database.Context.Models;
using EmployeeManagement.Database.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Database.Repositories.Implementations
{
    public class CustomerServiceRepository : GenericRepository<CustomerService2, Guid>, ICustomerServiceRepository
    {
        public CustomerServiceRepository(ISqlConnectionFactory sqlConnectionFactory) : base(sqlConnectionFactory)
        {

        }
    }
}
