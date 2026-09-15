using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Models.Email;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.CancelLeaveRequest
{
    public class CancelLeaveRequestCommandHandler :IRequestHandler<CancelLeaveRequestCommand,Unit>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IEmailSender _emailSender;

        public CancelLeaveRequestCommandHandler(ILeaveRequestRepository leaveRequestRepository,IEmailSender emailSender)
        {
            this._leaveRequestRepository = leaveRequestRepository;
            this._emailSender = emailSender;
        }

        public async Task<Unit> Handle(CancelLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            // get the required leave request
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(request.Id);
            if (leaveRequest == null) 
                throw new NotFoundException(nameof(leaveRequest),request.Id);

            // if exist cancel it
            leaveRequest.Cancelled = true;
            //send confirmation email
            var email = new EmailMessage
            {
                To = string.Empty,
                Body = $"Your request for {leaveRequest.StartDate:D} To {leaveRequest.EndDate:D}" +
                $"has been cancelled successfully.",
                Subject = "Leave Request Cancelled"
            };

            await _emailSender.SendEmail(email);
            return Unit.Value;
        }
    }
}
