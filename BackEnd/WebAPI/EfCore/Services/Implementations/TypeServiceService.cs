using AutoMapper;
using Base.Common.Interfaces;
using EmployeeManagement.Common.Event;
using EmployeeManagement.Database.Context.Models;
using EmployeeManagement.EfCore.Event;
using EmployeeManagement.EfCore.Services.Interfaces;
using EmployeeManagement.EfCore.ViewModels.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.EfCore.Services.Implementations
{
    public class TypeServiceService : BaseCRUDService<TypeService,TypeServiceRequestSearch, TypeService, Guid>, ITypeServiceService
    {
        public TypeServiceService(IServiceProvider serviceProvider) : base(serviceProvider)
        {

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
