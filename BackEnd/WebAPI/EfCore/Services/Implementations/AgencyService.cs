using AutoMapper;
using EmployeeManagement.Database.Context;
using EmployeeManagement.Database.Context.Models;
using EmployeeManagement.Database.Repositories.Implementations;
using EmployeeManagement.Database.Repositories.Interfaces;
using EmployeeManagement.EfCore.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.EfCore.Services.Implementations
{
    public class AgencyService: BaseService<Agency, Guid>,IAgencyService
    {
        private readonly IAgencyRepository _agencyRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LotteryService> _logger;

        public AgencyService(IConfiguration configuration, IMapper mapper, IRepository<Agency, Guid> repository, ApplicationDbContext db) : base(configuration, mapper, repository, db)
        {
        }


        public bool DeleteAgency(Agency lottery)
        {
            throw new NotImplementedException();
        }

        public List<Agency> GetListData(string name)
        {
            var listData = _agencyRepository.GetAll().ToList();
            try
            {
                if (name != "all")
                {
                    listData = listData.Where(c => c.NameAgency.ToUpper().Contains(name.ToUpper().Trim())).ToList();
                }
            }
            catch (Exception ex)
            {

                _logger.LogError("GetListData: " + ex.Message);

            }
            return listData;
        }

        public bool UpdateAgency(Agency lottery)
        {
            throw new NotImplementedException();
        }
    }
}
