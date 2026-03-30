namespace Template.Sample.Infrastructure.Persistence.EntityConfiguration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Extensions;
using Template.Sample.Domain.Entities;

/// <summary>
/// Entity Framework configuration for SampleItem.
/// </summary>
public class SampleItemConfiguration : IEntityTypeConfiguration<SampleItem>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<SampleItem> builder)
    {
        // Table/Collection name
        builder.ToCollection(SampleItem.CollectionName);

        // Primary key
        builder.HasKey(e => e.Id);

        // Properties
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Description)
            .HasMaxLength(1000);

        builder.Property(e => e.Category)
            .IsRequired();

        builder.Property(e => e.Status)
            .IsRequired();

        builder.Property(e => e.IsActive)
            .IsRequired();

        builder.Property(e => e.CreatedByUserId)
            .HasMaxLength(100);

        // Indexes
        builder.HasIndex(e => e.Name);
        builder.HasIndex(e => e.Category);
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.IsActive);
        builder.HasIndex(e => e.CreatedAt);
    }
}
