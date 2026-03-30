namespace Template.Shared.Infrastructure.Persistence.Extensions;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Extension methods for MongoDB-specific conventions.
/// </summary>
public static class MongoDbExtensions
{
    /// <summary>
    /// Applies MongoDB naming conventions to the model builder.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    /// <returns>The model builder for chaining.</returns>
    public static ModelBuilder UseMongoDbConventions(this ModelBuilder modelBuilder)
    {
        // MongoDB specific configurations can be added here
        // For example: setting default collection names, ID conventions, etc.

        return modelBuilder;
    }
}
