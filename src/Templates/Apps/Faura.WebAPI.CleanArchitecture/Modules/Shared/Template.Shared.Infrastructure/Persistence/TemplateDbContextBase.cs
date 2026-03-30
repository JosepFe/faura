namespace Template.Shared.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Template.Shared.Domain.Entities;
using Template.Shared.Infrastructure.Persistence.Extensions;

/// <summary>
/// Base class for all DbContexts in the application.
/// Provides common functionality for entity tracking and timestamp management.
/// </summary>
/// <param name="options">The options for this context.</param>
public class TemplateDbContextBase(DbContextOptions options) : DbContext(options)
{

    /// <inheritdoc/>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var addedEntities = ChangeTracker.Entries<EntityBase>()
            .Where(e => e.State == EntityState.Added)
            .Select(entry => entry.Entity);

        foreach (var entity in addedEntities)
        {
            entity.SetCreatedAt(DateTime.UtcNow);
            entity.SetUpdatedAt(DateTime.UtcNow);
        }

        var modifiedEntities = ChangeTracker.Entries<EntityBase>()
            .Where(e => e.State == EntityState.Modified)
            .Select(entry => entry.Entity);

        foreach (var entity in modifiedEntities)
        {
            entity.MarkAsUpdated();
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply MongoDB conventions if using MongoDB
        modelBuilder.UseMongoDbConventions();
    }
}
