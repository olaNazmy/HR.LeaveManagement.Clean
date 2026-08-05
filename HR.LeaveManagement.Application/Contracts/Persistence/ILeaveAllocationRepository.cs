using HR.LeaveManagement.Domain;

namespace HR.LeaveManagement.Application.Contracts.Persistence
{
    internal interface ILeaveAllocationRepository<T> : IGenericRepository<LeaveAllocation>
    { 
        Task<T> GetAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(T entity);

    }
}
