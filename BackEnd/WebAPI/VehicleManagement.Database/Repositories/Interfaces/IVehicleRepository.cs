using Base.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleManagement.Database.Models;

namespace VehicleManagement.Database.Repositories.Interfaces
{
    public interface IVehicleRepository : IRepositoryAsync<Vehicle, Guid>
    {

    }
}
