﻿using Base.Common.Service.Interfaces;
using EmployeeManagement.Database.Context.Models;
using EmployeeManagement.EfCore.ViewModels.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.EfCore.Services.Interfaces
{
    public interface ICustomerService: IBaseCRUDService<Customer,CustomerRequestSearch ,Customer, Guid>
    {
        public Task<Dictionary<Guid, string>> GetListCustomerById(List<Guid> id);
    }
}