using AutoMapper;
using EfCore.ViewModels;
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
          //  CreateMap<User, UserInfo>().ReverseMap();
        }
    }
}
