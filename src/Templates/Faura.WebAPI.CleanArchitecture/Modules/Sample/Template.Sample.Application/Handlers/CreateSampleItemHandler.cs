namespace Template.Sample.Application.Handlers;

using Faura.Infrastructure.Logger.Extensions;
using Faura.Infrastructure.Result;
using Microsoft.Extensions.Logging;
using Template.Sample.Application.Commands;
using Template.Sample.Application.DTOs;
using Template.Sample.Domain.Entities;
using Template.Sample.Domain.Repositories;
using Template.Shared.Application.SimpleMediator;

/// <summary>
/// Handler for creating a new sample item.
/// </summary>
/// <param name="repository">The sample item repository.</param>
/// <param name="logger">The logger.</param>
public class CreateSampleItemHandler(
    ISampleItemRepository repository,
    ILogger<CreateSampleItemHandler> logger) : IRequestHandler<CreateSampleItemCommand, FauraResult<SampleItemDto>>
{

    /// <inheritdoc/>
    public async Task<FauraResult<SampleItemDto>> Handle(
        CreateSampleItemCommand command,
        CancellationToken cancellationToken = default)
    {
        logger.LogFauraInformation("Creating sample item: {Name}", command.Name);

        // Validate name is not empty
        if (string.IsNullOrWhiteSpace(command.Name))
        {
            return FauraResult<SampleItemDto>.Error(
                new FauraError("SAMPLE_ITEM_NAME_REQUIRED", "Sample item name is required"));
        }

        // Check if name already exists
        var nameExists = await repository.ExistsByNameAsync(command.Name, cancellationToken: cancellationToken);
        if (nameExists)
        {
            return FauraResult<SampleItemDto>.Error(
                new FauraError("SAMPLE_ITEM_NAME_EXISTS", $"Sample item with name '{command.Name}' already exists"));
        }

        try
        {
            // Create the entity using factory method
            var sampleItem = SampleItem.Create(
                command.Name,
                command.Category,
                command.Description,
                command.CreatedByUserId);

            // Set tags if provided
            if (command.Tags != null && command.Tags.Any())
            {
                sampleItem.SetTags(command.Tags);
            }

            // Save to repository
            await repository.AddAsync(sampleItem, cancellationToken);

            // Map to DTO
            var dto = new SampleItemDto(
                sampleItem.Id,
                sampleItem.Name,
                sampleItem.Description,
                sampleItem.Category,
                sampleItem.Status,
                sampleItem.Tags,
                sampleItem.IsActive,
                sampleItem.CreatedByUserId,
                sampleItem.CreatedAt,
                sampleItem.UpdatedAt);

            logger.LogFauraInformation("Sample item created successfully: {Id}", sampleItem.Id);

            return FauraResult<SampleItemDto>.Success(dto);
        }
        catch (ArgumentException ex)
        {
            logger.LogFauraError(ex, "Validation error creating sample item");
            return FauraResult<SampleItemDto>.Error(
                new FauraError("VALIDATION_ERROR", ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogFauraError(ex, "Error creating sample item");
            return FauraResult<SampleItemDto>.Error(
                new FauraError("SAMPLE_ITEM_CREATE_ERROR", "An error occurred while creating the sample item"));
        }
    }
}
