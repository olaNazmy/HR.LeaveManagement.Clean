using HR.LeaveManagement.Domain;

namespace HR.LeaveManagement.Application.Contracts.Persistence
{
    public interface ILeaveTypeRepository :IGenericRepository<LeaveType>
    {
        Task<bool> IsLeaveTypeNameUnique(string name);

        //Task<T> GetAsync();
        //Task<T> GetByIdAsync(int id);
        //Task<T> CreateAsync(T entity);
        //Task<T> UpdateAsync(T entity);
        //Task<T> DeleteAsync(T entity);

    }
}
