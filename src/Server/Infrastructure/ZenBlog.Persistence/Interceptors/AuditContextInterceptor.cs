using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ZenBlog.Domain.Entities.Common;

namespace ZenBlog.Persistence.Interceptors;

public  class AuditContextInterceptor : SaveChangesInterceptor
{
    private static readonly Dictionary<EntityState, Action<DbContext, BaseEntity>> Behaviors = new(){
            { EntityState.Added, AddedBehavior },
            { EntityState.Modified, ModifiedBehavior }
        };
    private static void AddedBehavior(DbContext context, BaseEntity entity)
    {
        context.Entry(entity).Property(x=>x.UpdatedAt).IsModified=false;
        entity.CreatedAt = DateTime.Now;      
    }

    private static void ModifiedBehavior(DbContext context, BaseEntity entity)
    {
        context.Entry(entity).Property(x=>x.CreatedAt).IsModified=false;
        entity.UpdatedAt = DateTime.Now;      
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;

        if (context == null) return base.SavingChangesAsync(eventData, result, cancellationToken);

        var entries = context.ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            if (Behaviors.TryGetValue(entry.State, out var behavior))
            {
                behavior(context, entry.Entity);
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

}
