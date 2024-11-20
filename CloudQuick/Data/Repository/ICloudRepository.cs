using System.Linq.Expressions;

namespace CloudQuick.Data.Repository
{
    public interface ICloudRepository<T>
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(Expression<Func<T, bool>> filter, bool usenotracking = false);
        Task<T> GetByNameAsync(Expression<Func<T, bool>> filter); // Changed "Name" to "name"
        Task<T> CreateAsync(T dbRecord);
        Task<T> UpdateAsync(T dbRecord);
        Task<T> DeleteAsync(T dbRecord);
    }
}
