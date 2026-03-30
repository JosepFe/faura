namespace Template.Sample.Infrastructure.Persistence.Options;

/// <summary>
/// MongoDB configuration options for the Sample module.
/// </summary>
public class SampleMongoDbOptions
{
    /// <summary>
    /// Configuration section name.
    /// </summary>
    public const string SectionName = "Sample";

    /// <summary>
    /// Gets or sets the MongoDB settings.
    /// </summary>
    public required MongoDbSettings MongoDb { get; set; }

    /// <summary>
    /// MongoDB connection settings.
    /// </summary>
    public class MongoDbSettings
    {
        /// <summary>
        /// Gets or sets the MongoDB connection string.
        /// </summary>
        public string ConnectionString { get; set; } = null!;

        /// <summary>
        /// Gets or sets the database name.
        /// </summary>
        public string DatabaseName { get; set; } = null!;
    }
}
