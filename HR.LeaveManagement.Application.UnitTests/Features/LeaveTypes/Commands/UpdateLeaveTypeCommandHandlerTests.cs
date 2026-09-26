using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveType.Commands.UpdateLeaveType;
using HR.LeaveManagement.Application.MappingProfiles;
using HR.LeaveManagement.Application.UnitTests.Mocks;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.UnitTests.Features.LeaveTypes.Commands
{
    public class UpdateLeaveTypeCommandHandlerTests
    {
        private readonly Mock<ILeaveTypeRepository> _mockRepo;
        private readonly IMapper _mapper;
        private readonly Mock<IAppLogger<UpdateLeaveTypeCommandHandler>> _mockLogger;
        public UpdateLeaveTypeCommandHandlerTests()
        {
            _mockRepo = MockLeaveTypeRepository.GetLeaveTypeMockRepository();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<LeaveTypeProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
            _mockLogger = new Mock<IAppLogger<UpdateLeaveTypeCommandHandler>>();
        }

        [Fact]
        public async Task Handle_ExistingId_UpdatesLeaveType()
        {
            // Arrange
            var handler = new UpdateLeaveTypeCommandHandler(_mapper, _mockRepo.Object, _mockLogger.Object);

            var command = new UpdateLeaveTypeCommand
            {
                Id = 2,
                Name = "Updated Sick Leave",
                DefaultDays = 7
            };

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var updated = await _mockRepo.Object.GetByIdAsync(2);
            updated.Name.ShouldBe("Updated Sick Leave");
            updated.DefaultDays.ShouldBe(7);
        }

        [Fact]
        public async Task Handle_NonExistingId_ThrowsNotFoundException()
        {
            // Arrange
            var handler = new UpdateLeaveTypeCommandHandler(_mapper, _mockRepo.Object, _mockLogger.Object);

            var command = new UpdateLeaveTypeCommand
            {
                Id = 99,
                Name = "Does Not Exist",
                DefaultDays = 5
            };

            // Act & Assert
            await Should.ThrowAsync<NotFoundException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }

    }
}
