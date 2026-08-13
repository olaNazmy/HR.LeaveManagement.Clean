using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveType.Queries.GetLeaveTypeDetails
{
    public class GetLeaveTypesQuery
    {
        public record GetLeaveTypeDetailsQuery(int Id) : IRequest<LeaveTypeDetailsDto>;
    }
}
