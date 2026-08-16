using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveType.Commands.CreateLeaveType;

public class CreateLeaveTypeCommandValidator : AbstractValidator<CreateLeaveTypeCommand>
{
    public ILeaveTypeRepository _leaveTypeRepository;

    public CreateLeaveTypeCommandValidator( ILeaveTypeRepository leaveTypeRepository  )
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .NotNull()
            .MaximumLength(70).WithMessage("{PropertyName} must be fewer than 70 character");


        RuleFor(p => p.DefaultDays)
           .LessThan(1).WithMessage("{PropertyName} cannot less than 1")
           .GreaterThan(100).WithMessage("{PropertyName} cannot exceed 100");

        RuleFor(q => q)
           .MustAsync(LeaveTypeNameUnique)
           .WithMessage("Leave type already exist");
        //this allow add more complex validation
        _leaveTypeRepository = leaveTypeRepository;

        //

    }


    private  Task<bool> LeaveTypeNameUnique(CreateLeaveTypeCommand command, CancellationToken token)
    {
        return  _leaveTypeRepository.IsLeaveTypeNameUnique(command.Name);
    }
}
