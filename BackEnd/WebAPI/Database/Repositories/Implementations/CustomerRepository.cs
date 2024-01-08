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
    public class CustomerRepository : GenericRepository<Customer, Guid>, ICustomerRepository
    {
        public CustomerRepository(ISqlConnectionFactory sqlConnectionFactory) : base(sqlConnectionFactory)
        {

        }
    }
}
