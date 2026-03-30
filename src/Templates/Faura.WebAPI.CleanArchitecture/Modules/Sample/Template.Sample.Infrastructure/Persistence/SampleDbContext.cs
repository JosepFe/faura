namespace Template.Sample.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Template.Sample.Domain.Entities;
using Template.Sample.Infrastructure.Persistence.EntityConfiguration;
using Template.Shared.Infrastructure.Persistence;

/// <summary>
/// Database context for the Sample module.
/// </summary>
/// <param name="options">The database context options.</param>
public class SampleDbContext(DbContextOptions<SampleDbContext> options)
    : TemplateDbContextBase(options)
{

    /// <summary>
    /// Gets the SampleItems DbSet.
    /// </summary>
    public DbSet<SampleItem> SampleItems { get; init; }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new SampleItemConfiguration());
    }
}
