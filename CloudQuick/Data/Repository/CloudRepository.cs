using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace CloudQuick.Data.Repository
{
    public class CloudRepository<T> : ICloudRepository<T> where T : class
    {
        private readonly CloudDbContext _dbContext;
        private DbSet<T> _dbSet;

        public CloudRepository(CloudDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public async Task<T> CreateAsync(T dbRecord)
        {
            _dbSet.Add(dbRecord);
            await _dbContext.SaveChangesAsync();
            return dbRecord;
        }
        public async Task<T> DeleteAsync(T dbRecord)
        {
            if (dbRecord == null) throw new ArgumentNullException(nameof(dbRecord)); // Null-check for safety.

            _dbSet.Remove(dbRecord); // Remove the record from the DbSet.
            await _dbContext.SaveChangesAsync(); // Persist changes to the database.

            return dbRecord; // Return the deleted record.
        }



        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(Expression<Func<T, bool>> filter, bool usenotracking = false)
        {
            if (usenotracking)
                return await _dbSet.AsNoTracking().FirstOrDefaultAsync(filter);
            else
                return await _dbSet.FirstOrDefaultAsync(filter);
        }

        public async Task<T> GetByNameAsync(Expression<Func<T, bool>> filter) // Corrected return type to match the generic T.
        {
            return await _dbSet.FirstOrDefaultAsync(filter);
        }

        public async Task<T> UpdateAsync(T dbRecord)
        {
            _dbContext.Update(dbRecord);
            await _dbContext.SaveChangesAsync();
            return dbRecord;
        }
    }
}