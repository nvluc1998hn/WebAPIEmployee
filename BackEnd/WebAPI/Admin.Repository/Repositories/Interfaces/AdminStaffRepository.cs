using Admin.Domain.Entities;
using Admin.Repository.Repositories.Implementations;
using Base.Common.Dapper;
using Base.Common.Implementations;

namespace Admin.Repository.Repositories.Interfaces
{
    /// <summary>
    /// Report nhân viên MAUI
    /// </summary>
    /// Author: lucnv
    /// Created: 17/10/2024
    public class AdminStaffRepository : GenericRepository<AdminStaff, int>, IAdminStaffRepository
    {
        public AdminStaffRepository(ISqlConnectionFactory sqlConnectionFactory) : base(sqlConnectionFactory)
        {

        }
    }
}
