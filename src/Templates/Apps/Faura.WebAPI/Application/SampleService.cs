namespace Faura.WebAPI.Application;

using Faura.Infrastructure.Logger.Extensions;
using Faura.WebAPI.Domain;
using Faura.WebAPI.Domain.Entities;
using Faura.WebAPI.Infrastructure.Persistence;

/// <summary>
/// Service implementation for Sample operations.
/// Demonstrates business logic layer with repository and unit of work patterns.
/// Uses C# 12 primary constructor.
/// </summary>
public class SampleService(
    ILogger<SampleService> logger,
    ISampleRepository sampleRepository,
    ISampleUoW uoW) : ISampleService
{
    public async Task<IEnumerable<Sample>> GetSamplesAsync()
    {
        logger.LogFauraInformation("Starting Get Samples");
        return await sampleRepository.GetAsync();
    }

    public async Task<Sample?> GetSampleByIdAsync(long id)
    {
        logger.LogFauraInformation($"Getting sample by ID: {id}");
        return await sampleRepository.GetFirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Sample> CreateSampleAsync(string name, string description, string category)
    {
        logger.LogFauraInformation($"Creating sample: {name}");
        var sample = new Sample(name, description, category);
        var result = await sampleRepository.CreateAsync(sample);
        return result;
    }

    public async Task<Sample?> UpdateSampleAsync(long id, string name, string description, string category)
    {
        logger.LogFauraInformation($"Updating sample: {id}");
        
        var sample = await sampleRepository.GetFirstOrDefaultAsync(s => s.Id == id);
        if (sample == null)
        {
            logger.LogFauraWarning(null, $"Sample not found: {id}");
            return null;
        }

        sample.Update(name, description, category);
        await sampleRepository.UpdateAsync(sample);
        
        return sample;
    }

    public async Task<bool> DeleteSampleAsync(long id)
    {
        logger.LogFauraInformation($"Deleting sample: {id}");
        
        var sample = await sampleRepository.GetFirstOrDefaultAsync(s => s.Id == id);
        if (sample == null)
        {
            logger.LogFauraWarning(null, $"Sample not found: {id}");
            return false;
        }

        await sampleRepository.DeleteAsync(sample);
        return true;
    }

    public async Task<IEnumerable<Sample>> CreateMultipleSamplesWithTransactionAsync(
        string name1,
        string description1,
        string category1,
        string name2,
        string description2,
        string category2
    )
    {
        logger.LogFauraInformation("Creating multiple samples with transaction");
        
        var transaction = await uoW.GetDbTransaction();

        var sample1 = await sampleRepository.CreateAsync(
            new Sample(name1, description1, category1),
            detach: false,
            autoSaveChanges: false
        );
        
        var sample2 = await sampleRepository.CreateAsync(
            new Sample(name2, description2, category2),
            detach: false,
            autoSaveChanges: false
        );

        // You can add additional business logic here
        // send event 1
        // send event 2

        await uoW.CommitTransaction(transaction);

        return [sample1, sample2];
    }
}
