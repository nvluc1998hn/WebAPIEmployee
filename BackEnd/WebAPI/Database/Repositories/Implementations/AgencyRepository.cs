using EmployeeManagement.Database.Context;
using EmployeeManagement.Database.Context.Models;
using EmployeeManagement.Database.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Database.Repositories.Implementations
{
    public class AgencyRepository : Repository<Agency, Guid>, IAgencyRepository
    {
        public AgencyRepository(ApplicationDbContext context, ILogger<Agency> logger) : base(context, logger)
        {

        }
    }
}
