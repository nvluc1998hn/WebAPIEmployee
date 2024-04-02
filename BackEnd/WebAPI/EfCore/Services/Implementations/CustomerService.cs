using AutoMapper;
using Base.Common.Event;
using Base.Common.Interfaces;
using Base.Common.Services.Implementations;
using EmployeeManagement.Database.Context.Models;
using EmployeeManagement.EfCore.Services.Interfaces;
using EmployeeManagement.EfCore.ViewModels.Request;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.EfCore.Services.Implementations
{
    public class CustomerService : BaseCRUDService<Customer,CustomerRequestSearch, Customer, Guid>, ICustomerService
    {
        public CustomerService(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        /// <summary>
        /// Danh sách người dùng theo Id
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name Date Comments
        /// lucnv 11/01/2024 created
        /// </Modified>
        public async Task<Dictionary<Guid, string>> GetListCustomerById(List<Guid> id)
        {
            var listResult = new Dictionary<Guid, string>();
            try
            {
                var dataSQl = await _repository.GetListAsync($" WHERE CustomerId IN ({string.Join(",", id.Select(g => $"'{g}'"))})", "");

                if (dataSQl?.Count() > 0)
                {
                    listResult = dataSQl.ToDictionary(c => c.Id, c => c.FullName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"CustomerService_{System.Reflection.MethodBase.GetCurrentMethod().Name.ToString()}_{ex.ToString()}");

            }
            return listResult;
        }

        public override async Task<HandleResult<GridBaseResponse<Customer>>> GetPage(CustomerRequestSearch request)
        {
            var res = new HandleResult<GridBaseResponse<Customer>>() { Data = new(), Message = "Lấy dữ liệu không thành công"};

            try
            {
                var result = await _repository.GetListAsync();

                if (!string.IsNullOrEmpty(request.KeyWordSearch))
                {
                    result = result.Where(c=>c.FullName.Contains(request.KeyWordSearch) || c.Address.Contains(request.KeyWordSearch));
                }

                if (request.Month.HasValue)
                {
                    result = result.Where(c=> c.DateOfBirth.Month ==  request.Month.Value);
                }
                result = result.OrderByDescending(c=>c.CreatedDate).ToList();
                res.Success = true;
                res.Data.Items = result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"CustomerService_{System.Reflection.MethodBase.GetCurrentMethod().Name.ToString()}_{ex.ToString()}");
            }
            return res;
        }
    }
}
