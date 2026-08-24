using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Domain.Common;
using HR.LeaveManagement.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly HrDatabaseContext _databaseContext;

        // injecting the db context
        public GenericRepository(HrDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public async Task CreateAsync(T entity)
        {
            await _databaseContext.AddAsync(entity);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _databaseContext.Remove(entity);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync()
        {
            return await _databaseContext.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _databaseContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(q=>q.Id == id);
        }

        public async Task UpdateAsync(T entity)
        {
           _databaseContext.Update(entity);
            _databaseContext.Entry(entity).State = EntityState.Modified;
            await _databaseContext.SaveChangesAsync();
        }
    }
}
