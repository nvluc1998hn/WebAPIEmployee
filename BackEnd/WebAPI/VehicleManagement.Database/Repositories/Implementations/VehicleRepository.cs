using Base.Common.Dapper;
using Base.Common.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleManagement.Database.Models;
using VehicleManagement.Database.Repositories.Interfaces;

namespace VehicleManagement.Database.Repositories.Implementations
{
    public class VehicleRepository : GenericRepository<Vehicle, Guid>, IVehicleRepository
    {
        public VehicleRepository(ISqlConnectionFactory sqlConnectionFactory) : base(sqlConnectionFactory)
        {

        }
    }
}
