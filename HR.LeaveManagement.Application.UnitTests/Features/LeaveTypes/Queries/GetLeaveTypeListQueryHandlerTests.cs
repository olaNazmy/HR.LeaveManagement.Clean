using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;
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
    public class GetLeaveTypeListQueryHandlerTests
    {
        private readonly Mock<ILeaveTypeRepository> _mockRepo;
        private IMapper _mapper;
        private Mock<IAppLogger<GetAllLeaveTypesQueryHandler>> _mockApplogger;

        public GetLeaveTypeListQueryHandlerTests()
        {
            _mockRepo = MockLeaveTypeRepository.GetLeaveTypeMockRepository();

            // 
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<LeaveTypeProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
            _mockApplogger = new Mock<IAppLogger<GetAllLeaveTypesQueryHandler>>();

        }
        [Fact]
        public async Task GetLeaveTypeListTest()
        {
            var handler = new GetAllLeaveTypesQueryHandler(_mapper, _mockRepo.Object, _mockApplogger.Object);
            var result = await handler.Handle(new GetAllLeaveTypesQuery(), CancellationToken.None);
            
            //validate the result
            result.ShouldBeOfType<List<LeaveTypeDto>>();
            result.Count.ShouldBe(3);

        }
    }
}
