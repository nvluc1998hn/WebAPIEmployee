using AutoMapper;
using EfCore.ViewModels;
using EmployeeManagement.Common.Command.ActionCommand;
using EmployeeManagement.Database.Context.Models;
using EmployeeManagement.EfCore.Command.ActionCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfCore.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
             CreateMap<Employee, InsertEmployeeCommand>();
        }
    }
}
