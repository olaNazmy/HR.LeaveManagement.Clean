using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using MediatR;
using MediatR.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveType.Commands.UpdateLeaveType
{
    public class UpdateLeaveTypeCommandHandler : IRequestHandler<UpdateLeaveTypeCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        // we nee some dependencies
        public UpdateLeaveTypeCommandHandler(IMapper mapper, ILeaveTypeRepository leaveTypeRepository)
        {
            _mapper = mapper;
            _leaveTypeRepository = leaveTypeRepository;
        }

        public async Task<Unit> Handle(UpdateLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            // validate incoming data


            //convert to domain entity object
            var LeaveTypeToUpdate = _mapper.Map<Domain.LeaveType>(request);

            // update on db
            await _leaveTypeRepository.UpdateAsync(LeaveTypeToUpdate);
            // return updated record
            return Unit.Value;
           
        }
    }
}
