using Base.Common.Services.Implementations.Quartz;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.EfCore.DataInitializer
{
    public class DbInitializer : BaseDataInitializeV3, IDbInitializer
    {
        public DbInitializer(ILogger<BaseDataInitializeV3> logger, ISchedulerFactory schedulerFactory) : base(logger, schedulerFactory)
        {

        }

        protected override string SeedDataJob
        { get => "BA_GPS_Server.Maintain.Application.Jobs.StartingInstanceJob"; set { } }

    }
}
