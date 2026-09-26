using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.UnitTests.Mocks
{
    public class MockLeaveTypeRepository
    {
        public static Mock<ILeaveTypeRepository> GetLeaveTypeMockRepository()
        {
            var LeaveTypes = new List<LeaveType>
            {
                new LeaveType
                {
                    Id = 1,
                    DefaultDays = 10,
                    Name = "Test Vacation"
                },
                new LeaveType
                {
                    Id = 2,
                    DefaultDays = 5,
                    Name = "Test Sick"
                },
                new LeaveType
                {
                    Id = 3,
                    DefaultDays = 15,
                    Name = "Test Maternity"
                }
            };
            var mockRepo = new Mock<ILeaveTypeRepository>();
            // configure getting and creating LeaveType
            mockRepo.Setup(l => l.GetAsync()).ReturnsAsync(LeaveTypes);
            //setup create leaveType
            mockRepo.Setup(l => l.CreateAsync(It.IsAny<LeaveType>()))
                .Returns((LeaveType leaveType) =>
                {
                    LeaveTypes.Add(leaveType);
                    return Task.CompletedTask;
                });

            // setting up getting by id 
            mockRepo.Setup(l=> l.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync( (int id) => LeaveTypes.FirstOrDefault(q=>q.Id == id));

            // setting up update leave type
            mockRepo.Setup(l=> l.UpdateAsync(It.IsAny<LeaveType>()))
                .Returns ( (LeaveType leaveType) =>
                {
                    var exisiting = LeaveTypes.FirstOrDefault(q => q.Id == leaveType.Id);
                    if (exisiting != null)
                        LeaveTypes.Remove(leaveType);
                    LeaveTypes.Add(leaveType);
                    return Task.CompletedTask;
                });
            // setting up delete 
            mockRepo.Setup(l => l.DeleteAsync(It.IsAny<LeaveType>()))
    .Returns((LeaveType leaveType) =>
    {
        LeaveTypes.Remove(leaveType);
        return Task.CompletedTask;
    });

            return mockRepo;
            }
        }
    }

