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
    public class LeaveAllocationRepository :GenericRepository<LeaveAllocation>,ILeaveAllocationRepository
    {
        public LeaveAllocationRepository(HrDatabaseContext context):base(context)
        {
            
        }

        public async Task AddAllocations(List<LeaveAllocation> allocations)
        {

            await _databaseContext.AddRangeAsync(allocations);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<bool> AllocationExists(string userId, int leaveTypeId, int period)
        {
           return await _databaseContext.LeaveAllocations.AnyAsync(
           q => q.EmployeeId == userId
            && q.LeaveTypeId == leaveTypeId 
            && q.Period == period);
        }

        public async Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails()
        {
            var leavAllocations = await _databaseContext.LeaveAllocations
                .Include(q => q.leaveType).
                ToListAsync();
            return leavAllocations;
        }

        public async Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails(string userId)
        {
            var leavAllocations = await _databaseContext.LeaveAllocations.Where(q => q.EmployeeId == userId)
                .Include(q => q.leaveType)
                .ToListAsync();
            return leavAllocations;
        }

        public async Task<LeaveAllocation> GetLeaveAllocationWithDetails(int id)
        {
            var leaveAllocation = await _databaseContext.LeaveAllocations
                .Include(q=>q.leaveType)
                .FirstOrDefaultAsync(q=>q.Id == id);

            return leaveAllocation;
        }

        public async Task<LeaveAllocation> GetUserAllocations(string userId, int leaveTypeId)
        {
           return await _databaseContext.LeaveAllocations.FirstOrDefaultAsync(q=>q.EmployeeId == userId 
           && q.LeaveTypeId == leaveTypeId);
        }
        //
    }
}
