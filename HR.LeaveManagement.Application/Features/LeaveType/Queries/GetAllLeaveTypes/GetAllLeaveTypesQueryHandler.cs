using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes
{
    public class GetAllLeaveTypesQueryHandler : IRequestHandler<GetAllLeaveTypesQuery, List<LeaveTypeDto>>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        // we nee some dependencies
        public GetAllLeaveTypesQueryHandler(IMapper mapper, ILeaveTypeRepository leaveTypeRepository)
        {
            this._mapper = mapper;
            this._leaveTypeRepository = leaveTypeRepository;
        }
        public async Task<List<LeaveTypeDto>> Handle(GetAllLeaveTypesQuery request, CancellationToken cancellationToken)

        {
            // query the db 
            var leaveTypes = await _leaveTypeRepository.GetAsync();

            // convert the data objects to dto objects
            var data = _mapper.Map<List<LeaveTypeDto>>(leaveTypes);
            //return list of dto objects
            return data;
        }
    }
}
