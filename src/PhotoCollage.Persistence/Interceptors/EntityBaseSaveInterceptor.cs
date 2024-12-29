using Microsoft.EntityFrameworkCore.Diagnostics;
using PhotoCollage.Core.Entities;

namespace PhotoCollage.Persistence.Interceptors;

internal sealed class EntityBaseSaveInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is not null)
        {
            this.UpdateEntityBase(eventData.Context);
        }

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            this.UpdateEntityBase(eventData.Context);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntityBase(DbContext context)
    {
        var entries = context.ChangeTracker.Entries<EntityBase>().ToArray();
        foreach (var entry in entries)
        {
            if (entry.State != EntityState.Added
                && entry.State != EntityState.Modified)
            {
                continue;
            }

            var now = DateTimeOffset.UtcNow;
            entry.Property(nameof(EntityBase.DateModified)).CurrentValue = now;
            if (entry.State == EntityState.Added)
            {
                entry.Property(nameof(EntityBase.DateCreated)).CurrentValue = now;
            }
        }
    }
}
