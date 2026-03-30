using Faura.WebAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Faura.WebAPI.Infrastructure.Persistence;

/// <summary>
/// Database context for Sample entities.
/// Configures database connection and entity mappings.
/// </summary>
public class SampleDbContext : DbContext
{
    public SampleDbContext(DbContextOptions<SampleDbContext> options)
        : base(options) { }

    public DbSet<Sample> Samples { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Sample>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Category).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Name);
        });
    }
}
