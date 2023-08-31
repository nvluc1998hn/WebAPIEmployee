using AutoMapper;
using EmployeeManagement.Database.Context;
using EmployeeManagement.Database.Context.Models;
using EmployeeManagement.Database.Repositories.Implementations;
using EmployeeManagement.Database.Repositories.Interfaces;
using EmployeeManagement.EfCore.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.EfCore.Services.Implementations
{
    public class BaseService<T, Id> : IBaseService<T, Id> where T : class
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IRepository<T, Id> _repository;

        public BaseService(IConfiguration configuration, IMapper mapper, IRepository<T, Id> repository, ApplicationDbContext db)
        {

            _configuration = configuration;
            _mapper = mapper;
            _repository = repository;
        }

        public bool Add(T data)
        {
            bool isSuccess = false;
            try
            {
                PropertyInfo primaryKeyProperty = typeof(T).GetProperties()
                .FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(KeyAttribute)));

                var idInsert = _repository.Insert(data, primaryKeyProperty.Name);
                if (idInsert != null)
                {
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {

                isSuccess = false;
            }
            return isSuccess;
        }

        //public bool Delete(T lottery)
        //{
        //    throw new NotImplementedException();
        //}

        //public bool Update(T data)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
