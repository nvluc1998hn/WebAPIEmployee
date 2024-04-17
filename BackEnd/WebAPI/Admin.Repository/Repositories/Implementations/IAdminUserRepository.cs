using Admin.Domain.Entities;
using Base.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Repository.Repositories.Implementations
{
    public interface IAdminUserRepository : IRepositoryAsync<AdminUser, Guid>
    {

    }
}
