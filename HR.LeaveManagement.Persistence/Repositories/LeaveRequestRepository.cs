using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Domain;
using HR.LeaveManagement.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Persistence.Repositories
{
    public class LeaveRequestRepository :GenericRepository<LeaveRequest>,ILeaveRequestRepository
    {
        public LeaveRequestRepository(HrDatabaseContext databaseContext) : base(databaseContext)
        {

        }

        public async Task<LeaveRequest> GetLeaveRequestWithDetails(int id)
        {
            var leaveRequest = await _databaseContext.leaveRequests
                .Include(q => q.LeaveType)
                .FirstOrDefaultAsync(q=>q.Id == id);

            return leaveRequest;
        }

        public async Task<List<LeaveRequest>> GetLeaveRequestWithDetails()
        {
            var leavRequests = await _databaseContext.leaveRequests
                 .Include(q => q.LeaveType)
                 .ToListAsync();

            return leavRequests;
        }

        public async Task<List<LeaveRequest>> GetLeaveRequestWithDetails(string userId)
        {
            var leavRequests = await _databaseContext.leaveRequests
                .Where(q=> q.RequestingEmployeeId == userId)
                .Include(q=>q.LeaveType)
                .ToListAsync();
            
            return leavRequests;
        }
        //
    }
}
