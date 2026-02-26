namespace Backend.Domain.Common;

public interface IRepository<T> where T : class
{
   Task<T> CreateAsync(T entity);
   Task<IEnumerable<T>> GetAllAsync();
   Task<T?> GetByIdAsync(Guid id);
   Task<T> UpdateAsync(T entity);
}