using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes
{
    //public class GetAllLeaveTypesQuery : IRequest<List<LeaveTypeDto>>
    //{
    //    create cqrs query

    //}
    public record GetAllLeaveTypesQuery : IRequest<List<LeaveTypeDto>>;

}
