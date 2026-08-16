using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveType.Commands.DeleteLeaveType
{
    public class DeleteLeaveTypeCommandHandler : IRequestHandler<DeleteLeaveTypeCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        // we nee some dependencies
        public DeleteLeaveTypeCommandHandler(IMapper mapper, ILeaveTypeRepository leaveTypeRepository)
        {
            _mapper = mapper;
            _leaveTypeRepository = leaveTypeRepository;
        }
        public async Task<Unit> Handle(DeleteLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            // convert the domain entity to object
            var LeaveTypeToDelete = await _leaveTypeRepository.GetByIdAsync(request.Id);

            //verify the record exist
            if (LeaveTypeToDelete == null)
            {
                throw new NotFoundException(nameof(LeaveType),request.Id);
            }
            // delete from db
            await _leaveTypeRepository.DeleteAsync(LeaveTypeToDelete);

            return Unit.Value;
        }
    }
}
