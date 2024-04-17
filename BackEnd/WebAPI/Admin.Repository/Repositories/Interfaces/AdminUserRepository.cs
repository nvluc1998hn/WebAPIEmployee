using Admin.Domain.Entities;
using Admin.Repository.Repositories.Implementations;
using Base.Common.Dapper;
using Base.Common.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Repository.Repositories.Interfaces
{
    public class AdminUserRepository : GenericRepository<AdminUser, Guid>, IAdminUserRepository
    {
        public AdminUserRepository(ISqlConnectionFactory sqlConnectionFactory) : base(sqlConnectionFactory)
        {

        }
    }
}
