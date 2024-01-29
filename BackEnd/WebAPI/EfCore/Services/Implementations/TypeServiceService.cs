using AutoMapper;
using Base.Common.Event;
using Base.Common.Interfaces;
using Base.Common.Services.Implementations;
using EmployeeManagement.Database.Context.Models;
using EmployeeManagement.EfCore.Services.Interfaces;
using EmployeeManagement.EfCore.ViewModels.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.EfCore.Services.Implementations
{
    /// <summary>
    /// 
    /// </summary>
    /// <Modified>
    /// Name Date Comments
    /// lucnv 11/01/2024 created
    /// </Modified>
    public class TypeServiceService : BaseCRUDService<TypeService,TypeServiceRequestSearch, TypeService, Guid>, ITypeServiceService
    {
        public TypeServiceService(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        public async Task<Dictionary<Guid, TypeService>> GetListTypeServiceById(List<Guid> Ids)
        {
            var listResult = new Dictionary<Guid, TypeService>();
            try
            {
                var dataSQl = await _repository.GetListAsync($" WHERE TypeServiceId IN ({string.Join(",", Ids.Select(g => $"'{g}'"))})", "");

                if (dataSQl?.Count() > 0)
                {
                    listResult = dataSQl.ToDictionary(c => c.TypeServiceId, c => c);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return listResult;
        }

        public override async Task<HandleResult<GridBaseResponse<TypeService>>> GetPage(TypeServiceRequestSearch request)
        {
            var res = new HandleResult<GridBaseResponse<TypeService>>() { Data = new(), Message = "Lấy dữ liệu không thành công" };

            try
            {
                if (string.IsNullOrEmpty(request.KeyWordSearch))
                {
                    // Nếu KeyWordSearch là null hoặc rỗng, không truyền điều kiện tìm kiếm
                    res.Data.Items = (List<TypeService>)await _repository.GetListAsync();
                }
                else
                {
                    // Nếu KeyWordSearch có giá trị, chỉ truyền điều kiện cho TypeServiceName
                    res.Data.Items = (List<TypeService>)await _repository.GetListAsync(new { TypeServiceName = request.KeyWordSearch });
                }
                res.Success = true;
                res.Data.Items = res.Data.Items.OrderByDescending(res => res.DateApply).ToList();
            }
            catch (Exception)
            {

                throw;
            }

            return res;
        }
    }
}
