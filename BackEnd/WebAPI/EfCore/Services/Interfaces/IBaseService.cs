using EmployeeManagement.Database.Context.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.EfCore.Services.Interfaces
{
    public interface IBaseService<T, Id> where T : class
    {
        bool Add(T data);

        bool Update(T data);

        bool Delete(T data);

    }
}
