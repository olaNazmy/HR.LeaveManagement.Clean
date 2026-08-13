using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HR.LeaveManagement.Application.Features.LeaveType.Queries.GetLeaveTypeDetails.GetLeaveTypesQuery;

namespace HR.LeaveManagement.Application.Features.LeaveType.Queries.GetLeaveTypeDetails
{
    public class GetLeaveTypeDetailsQueryHandler : IRequestHandler<GetLeaveTypeDetailsQuery, LeaveTypeDetailsDto>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveTypeRepository _leaveTypeRepository;

        public GetLeaveTypeDetailsQueryHandler(IMapper mapper, ILeaveTypeRepository leaveTypeRepository)
        {
            _mapper = mapper;
            _leaveTypeRepository = leaveTypeRepository;
        }

        public async Task<LeaveTypeDetailsDto> Handle(GetLeaveTypeDetailsQuery request,CancellationToken cancellationToken)
        {
            // query the db 
            var leaveTypes = await _leaveTypeRepository.GetByIdAsync(request.Id);
            return _mapper.Map<LeaveTypeDetailsDto>(leaveTypes);
        }
    }
}
