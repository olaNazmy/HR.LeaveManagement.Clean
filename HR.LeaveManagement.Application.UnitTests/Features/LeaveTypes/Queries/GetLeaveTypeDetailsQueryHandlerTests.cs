using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetLeaveTypeDetails;
using HR.LeaveManagement.Application.MappingProfiles;
using HR.LeaveManagement.Application.UnitTests.Mocks;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.UnitTests.Features.LeaveTypes.Queries
{
    public class GetLeaveTypeDetailsQueryHandlerTests
    {
        private readonly Mock<ILeaveTypeRepository> _mockRepo;
        private readonly IMapper _mapper;

        public GetLeaveTypeDetailsQueryHandlerTests()
        {
            _mockRepo = MockLeaveTypeRepository.GetLeaveTypeMockRepository();
            var mapperConfig = new MapperConfiguration(c=>
            {
                c.AddProfile<LeaveTypeProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task Handle_ExistingId_ReturnsLeaveTypeDetails()
        {
            //Arrange
            var handler =  new GetLeaveTypeDetailsQueryHandler(_mapper,_mockRepo.Object);

            //Act
            var result = await handler.Handle(
                       new GetLeaveTypeDetailsQuery(2), CancellationToken.None);

            //Assert
            result.ShouldBeOfType<LeaveTypeDetailsDto>();
           result.Name.ShouldBe("Test Sick");
           //result.Name.ShouldBe("Test Vacation"); for test the fail only 
            result.DefaultDays.ShouldBe(5);
        }
    }
}
