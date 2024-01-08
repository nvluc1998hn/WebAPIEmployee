using Autofac.Core;
using EmployeeManagement.Database.Context.Models;
using EmployeeManagement.EfCore.Services.Interfaces;
using EmployeeManagement.EfCore.ViewModels.Request;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EmployeeManagementAPI.Controllers
{
    [Route("api/v1/type-service")]
    public class TypeServiceController : GridBaseCRUDController<TypeService, TypeServiceRequestSearch, TypeService, ITypeServiceService>
    {
        public TypeServiceController(IServiceProvider provider) : base(provider)
        {

        }
    }
}
