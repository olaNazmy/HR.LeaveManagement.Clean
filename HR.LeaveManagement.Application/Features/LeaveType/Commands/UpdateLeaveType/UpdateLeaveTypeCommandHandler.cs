using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
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
        private readonly IAppLogger<UpdateLeaveTypeCommandHandler> _logger;

        // we nee some dependencies
        public UpdateLeaveTypeCommandHandler(IMapper mapper, ILeaveTypeRepository leaveTypeRepository,
            IAppLogger<UpdateLeaveTypeCommandHandler> logger)
        {
            this._mapper = mapper;
            this._leaveTypeRepository = leaveTypeRepository;
            this._logger = logger;
        }


        public async Task<Unit> Handle(UpdateLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            //// validate incoming data
            //var validator = new UpdateLeaveTypeCommandValidator(_leaveTypeRepository);
            //var validationResult = await validator.ValidateAsync(request);

            //if (validationResult.Errors.Any()) 
            //{
            //    // we can add error logging 
            //    _logger.LogWarning("validation error in update request for {0} - {1}", nameof(LeaveType), request.Id);
            //    throw new BadRequestException("invalid Leave Type", validationResult);
            //}


            //convert to domain entity object
            var leaveTypeToUpdate = await _leaveTypeRepository.GetByIdAsync(request.Id);
            _mapper.Map(request, leaveTypeToUpdate);

            // update on db
            await _leaveTypeRepository.UpdateAsync(leaveTypeToUpdate);
            //
            _logger.LogInformation("LeaveType {LeaveTypeId} updated successfully", request.Id);

            // return updated record
            return Unit.Value;
           
        }
    }
}
