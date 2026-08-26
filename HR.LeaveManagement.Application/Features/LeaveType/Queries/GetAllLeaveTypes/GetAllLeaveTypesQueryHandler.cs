using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetLeaveTypeDetails;
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
        private readonly IAppLogger<GetLeaveTypeDetailsQueryHandler> _logger;

        // we nee some dependencies
        public GetAllLeaveTypesQueryHandler(IMapper mapper, ILeaveTypeRepository leaveTypeRepository,
            IAppLogger<GetLeaveTypeDetailsQueryHandler> logger)
        {
            this._mapper = mapper;
            this._leaveTypeRepository = leaveTypeRepository;
            this._logger = logger;
        }
        public async Task<List<LeaveTypeDto>> Handle(GetAllLeaveTypesQuery request, CancellationToken cancellationToken)

        {
            // query the db 
            var leaveTypes = await _leaveTypeRepository.GetAsync();

            // convert the data objects to dto objects
            var data = _mapper.Map<List<LeaveTypeDto>>(leaveTypes);
            //return list of dto objects
            // we can use logger anywhere
            _logger.LogInformation("Leave types retrieved successfully");
            return data;
        }
    }
}
