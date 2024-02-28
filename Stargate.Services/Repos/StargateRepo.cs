using Microsoft.EntityFrameworkCore;
using Stargate.Services.Data;
using System.Linq.Expressions;

namespace Stargate.Services.Repos;
public interface IRepository<T>
{
    Task<T> GetAsync(int id, CancellationToken cancellationToken = default);
    Task<T> GetAsync(string username, CancellationToken cancellationToken = default);
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(T entity, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> AllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<bool> SaveChangesAsync();
}

public abstract class StargateRepo<T> : IRepository<T> where T : class
{
    protected StargateContext _context;

    public StargateRepo(StargateContext context)
    {
        _context = context;
    }

    public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _context.AddAsync(entity, cancellationToken);
        await SaveChangesAsync();
        return entity;
    }

    public async Task<IEnumerable<T>> AllAsync() => await _context.Set<T>().ToListAsync();

    public virtual async Task<bool> DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _context.Remove(entity);

        return await SaveChangesAsync();
    }

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) =>
        await _context.Set<T>()
            .AsQueryable().Where(predicate).ToListAsync();

    public virtual async Task<T> GetAsync(int id, CancellationToken cancellationToken = default) => await _context.FindAsync<T>(id);

    public virtual async Task<T> GetAsync(string username, CancellationToken cancellationToken = default) => await _context.FindAsync<T>(username);

    public virtual async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;

    public virtual async Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _context.Update(entity);

        return await SaveChangesAsync();
    }
}