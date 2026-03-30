using System.ComponentModel.DataAnnotations.Schema;

namespace Faura.WebAPI.Domain.Entities;

/// <summary>
/// Sample entity for demonstration purposes.
/// Use this as a template for creating your own entities.
/// </summary>
[Table("sample")]
public class Sample
{
    public Sample(string name, string description, string category)
    {
        Name = name;
        Description = description;
        Category = category;
    }

    [Column("id")]
    public long Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("description")]
    public string Description { get; set; }

    [Column("category")]
    public string Category { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Updates the sample properties.
    /// </summary>
    public void Update(string name, string description, string category)
    {
        Name = name;
        Description = description;
        Category = category;
    }
}
