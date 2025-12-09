using System.Linq.Expressions;
using ZenBlog.Domain.Entities.Common;

namespace ZenBlog.Application.Contracts.Persistence;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    Task<List<TEntity>> GetAllAsync(bool tracking = false, CancellationToken cancellationToken = default);
    IQueryable<TEntity> Where(bool tracking = false, params Expression<Func<TEntity, object>>[] includes);
     Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<TEntity?> GetSingleAsync(Expression<Func<TEntity, bool>> filter, bool tracking = false, CancellationToken cancellationToken = default);
    Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}
