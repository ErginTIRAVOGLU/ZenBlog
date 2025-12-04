using System;
using System.Linq.Expressions;
using ZenBlog.Domain.Entities.Common;

namespace ZenBlog.Application.Contracts.Persistence;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    Task<List<TEntity>> GetAllAsync(bool tracking = false,CancellationToken cancellationToken = default);
    IQueryable<TEntity> Where(bool tracking = false);
    Task<TEntity?> GetByIdAsync(Guid id,CancellationToken cancellationToken = default);
    Task<TEntity?> GetSingleAsync(Expression<Func<TEntity, bool>> filter, bool tracking = false,CancellationToken cancellationToken = default);
    Task CreateAsync(TEntity entity,CancellationToken cancellationToken = default);
    void UpdateAsync(TEntity entity);
    void DeleteAsync(TEntity entity);
}
