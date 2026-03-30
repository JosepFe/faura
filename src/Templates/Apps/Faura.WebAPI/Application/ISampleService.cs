using Faura.WebAPI.Domain.Entities;

namespace Faura.WebAPI.Application;

/// <summary>
/// Service interface for Sample operations.
/// </summary>
public interface ISampleService
{
    /// <summary>
    /// Gets all samples.
    /// </summary>
    Task<IEnumerable<Sample>> GetSamplesAsync();

    /// <summary>
    /// Gets a sample by ID.
    /// </summary>
    Task<Sample?> GetSampleByIdAsync(long id);

    /// <summary>
    /// Creates a new sample.
    /// </summary>
    Task<Sample> CreateSampleAsync(string name, string description, string category);

    /// <summary>
    /// Updates an existing sample.
    /// </summary>
    Task<Sample?> UpdateSampleAsync(long id, string name, string description, string category);

    /// <summary>
    /// Deletes a sample.
    /// </summary>
    Task<bool> DeleteSampleAsync(long id);

    /// <summary>
    /// Creates multiple samples within a transaction.
    /// Demonstrates transactional behavior - if one fails, all are rolled back.
    /// </summary>
    Task<IEnumerable<Sample>> CreateMultipleSamplesWithTransactionAsync(
        string name1,
        string description1,
        string category1,
        string name2,
        string description2,
        string category2
    );
}
