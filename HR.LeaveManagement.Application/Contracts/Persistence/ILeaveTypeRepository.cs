using HR.LeaveManagement.Domain;

namespace HR.LeaveManagement.Application.Contracts.Persistence
{
    public interface ILeaveTypeRepository : IGenericRepository<LeaveType>
    {
        Task<bool> LeaveTypeExists(string name, int excludeId);
        Task<bool> LeaveTypeExists(string name);
    }
}
