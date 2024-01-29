using Base.Common.Event;
using EmployeeManagement.Common.Command.ActionCommand;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeManagement.EfCore.Command.ActionCommand
{
    public class EmployeeCommandHandler : IRequestHandler<InsertEmployeeCommand, HandleResult>
    {
        public EmployeeCommandHandler() { 
            
        }
        public Task<HandleResult> Handle(InsertEmployeeCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
