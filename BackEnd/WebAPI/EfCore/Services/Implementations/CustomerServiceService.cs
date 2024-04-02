using Base.Common.Event;
using Base.Common.Helper;
using Base.Common.Interfaces;
using Base.Common.Respone;
using Base.Common.Services.Implementations;
using EmployeeManagement.Database.Context.Models;
using EmployeeManagement.EfCore.Services.Interfaces;
using EmployeeManagement.EfCore.ViewModels.Request;
using EmployeeManagement.EfCore.ViewModels.Response;
using EventBusRabbitMQ.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace EmployeeManagement.EfCore.Services.Implementations
{
    /// <summary>
    /// Dịch vụ - khách hàng
    /// </summary>
    /// <Modified>
    /// Name Date Comments
    /// lucnv 10/01/2024 created
    /// </Modified>
    public class CustomerServiceService : GridBaseService<CustomerService2, string>, ICustomerServiceService
    {
        public ReturnType returnType =new ReturnType();
        public readonly ICustomerService _customerService;
        public readonly ITypeServiceService _typeServiceService;
       

        public CustomerServiceService(IServiceProvider provider, ICustomerService customerService, ITypeServiceService typeServiceService) : base(provider)
        {
            _customerService = customerService;
            _typeServiceService = typeServiceService;
        }     

        
        public async Task<HandleResult<GridBaseResponse<CustomerServiceViewModel>>> GetPage(CustomerServiceRequest request)
        {
            var res = new HandleResult<GridBaseResponse<CustomerServiceViewModel>>() { Data = new(), Message = "Lấy dữ liệu không thành công" };

            try
            {
                // Test rabbitMQ

                //var func = (List<CustomerService2> entities) =>
                //{

                //};

                //RabbitMqSyncDataHelper.SubscribeSyncInstance(func);

                var dataMap = new List<CustomerServiceViewModel>();

                string conditions = $" WHERE InvoiceDate BETWEEN '{DatetimeHelper.ToSqlDatetime((DateTime)request.FromDate)}' AND '{DatetimeHelper.ToSqlDatetime((DateTime)request.ToDate)}' ";


                var listDataDB = await _repository.GetListAsync(conditions,null,null,null);

                if (listDataDB.ToList()?.Count > 0)
                {
                    var listCustomer = listDataDB.Select(c => c.CustomerId).Distinct().ToList();
                    var listTypeService = listDataDB.Select(c => c.TypeServiceId).Distinct().ToList();

                    var dicCustomer = await _customerService.GetListCustomerById(listCustomer);
                    var dicTypeService = await _typeServiceService.GetListTypeServiceById(listTypeService);


                    foreach (var item in listDataDB)
                    {

                        CustomerServiceViewModel customerServiceViewModel = new CustomerServiceViewModel();
                        customerServiceViewModel.TypeServiceId = item.TypeServiceId;
                        customerServiceViewModel.CustomerId = item.CustomerId;
                        customerServiceViewModel.CustomerName = dicCustomer.TryGetValue(item.CustomerId, out var value3) ? value3.ToString() : null;
                        customerServiceViewModel.InvoiceDate = item.InvoiceDate;
                        customerServiceViewModel.TypeServiceName = dicTypeService.TryGetValue(item.TypeServiceId, out var value) ? value.TypeServiceName : null;
                        customerServiceViewModel.Price = dicTypeService.TryGetValue(item.TypeServiceId, out var value2) ? value2.Price : 0;
                        dataMap.Add(customerServiceViewModel);
                    }
                }

                if (!string.IsNullOrEmpty(request.KeyWordSearch))
                {           
                    dataMap = dataMap.Where(c=>c.CustomerName.Contains(request.KeyWordSearch)).ToList();
                }
                res.Data.Items = dataMap;

                res.Success = true;
            }
            catch (Exception)
            {

                throw;
            }

            return res;

        }

        public async Task<ReturnType> InsertListCustomerService(List<CustomerService2> listDataInsert)
        {
            returnType.Status = false;
            try
            {
                var dataInsert = await  _repository.AddAsync(listDataInsert);
                if(dataInsert != null)
                {
                    returnType.Status = true;   
                }
            }
            catch (Exception)
            {

                throw;
            }
            return returnType;

        }

        public async Task<ReturnType> UpdateListCustomerService(List<CustomerService2> listDataInsert)
        {
            returnType.Status = false;
            try
            {
                if (listDataInsert?.Count > 0)
                {

                    foreach (var item in listDataInsert)
                    {
                        item.UpdatedDate = DateTime.Now;
                    }
                }
                var dataUpdate = await _repository.UpdateAsync(listDataInsert);
                if (dataUpdate != null)
                {
                    returnType.Status = true;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return returnType;

        }

        public async Task<ReturnType> DeletCustomerService(CustomerService2 customerService)
        {
            returnType.Status = false;
            try
            {
                if (customerService !=null)
                {
                    var delete = await _repository.DeleteAsync(customerService);
                    if (delete != null)
                    {
                        returnType.Status = true;
                    }
                }   
            }
            catch (Exception)
            {

                throw;
            }
            return returnType;
        }

        public async Task<ReturnType> DeleteListCustomerService(List<CustomerService2> listDataInsert)
        {
            returnType.Status = false;
            try
            {
                if (listDataInsert?.Count > 0)
                {
                    var delete = await _repository.DeleteAsync(listDataInsert);
                    if (delete != null)
                    {
                        returnType.Status = true;
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
            return returnType;
        }

        public async Task<ReturnType> DeleteMulti(CustomerServiceDeleteRequest rq)
        {
            returnType.Status = false;
            try
            {
                if (rq.CustomerId!= null)
                {
                    var listByUser =await _repository.GetListAsync(new { CustomerId = rq.CustomerId });
                   
                    listByUser = listByUser.Where(c => c.InvoiceDate.Date == rq.InvoiDate.Date).ToList();
                  
                    if(listByUser.Count() > 0)
                    {
                        foreach (var item in listByUser)
                        {
                            await _repository.DeleteAsync(item);    
                        }    
                        returnType.Status = true;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return returnType;
        }
    }
}
