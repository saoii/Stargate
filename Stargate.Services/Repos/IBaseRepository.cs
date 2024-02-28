namespace Stargate.Services.Repos;

public interface IBaseRepository<T>
{
    Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<T> GetByNameAsync(string username, CancellationToken cancellationToken = default);
    Task<List<T>> ListAllAsync();
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task<int> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task<int> DeleteAsync(T entity, CancellationToken cancellationToken = default);
    IQueryable<T> ListAll();

}
