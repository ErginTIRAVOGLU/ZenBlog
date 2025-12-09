using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Domain.Entities.Common;
using ZenBlog.Persistence.Context;

namespace ZenBlog.Persistence.Concrete;

public sealed class GenericRepository<TEntity>(AppDbContext context) : IRepository<TEntity> where TEntity : BaseEntity
{
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();


    public async Task<List<TEntity>> GetAllAsync(bool tracking = false, CancellationToken cancellationToken = default)
    {
        return tracking ? await _dbSet.ToListAsync(cancellationToken) : await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync(id, cancellationToken);
    }

    public async Task<TEntity?> GetSingleAsync(Expression<Func<TEntity, bool>> filter, bool tracking = false, CancellationToken cancellationToken = default)
    {
        return tracking ? await _dbSet.FirstOrDefaultAsync(filter, cancellationToken) : await _dbSet.AsNoTracking().FirstOrDefaultAsync(filter, cancellationToken);
    }

    public async Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public IQueryable<TEntity> Where(bool tracking = false, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = tracking ? _dbSet.AsQueryable() : _dbSet.AsNoTracking();
        
        if (includes != null && includes.Length > 0)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        
        return query;
    }
 
}
