using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Persistence.Context;

namespace ZenBlog.Persistence.Concrete;

public sealed class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken);    
    }
}
