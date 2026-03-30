namespace Template.Sample.Application.Handlers;

using Faura.Infrastructure.Logger.Extensions;
using Faura.Infrastructure.Result;
using Microsoft.Extensions.Logging;
using Template.Sample.Application.Commands;
using Template.Sample.Application.DTOs;
using Template.Sample.Domain.Repositories;
using Template.Shared.Application.SimpleMediator;

/// <summary>
/// Handler for updating an existing sample item.
/// </summary>
/// <param name="repository">The sample item repository.</param>
/// <param name="logger">The logger.</param>
public class UpdateSampleItemHandler(
    ISampleItemRepository repository,
    ILogger<UpdateSampleItemHandler> logger) : IRequestHandler<UpdateSampleItemCommand, FauraResult<SampleItemDto>>
{

    /// <inheritdoc/>
    public async Task<FauraResult<SampleItemDto>> Handle(
        UpdateSampleItemCommand command,
        CancellationToken cancellationToken = default)
    {
        logger.LogFauraInformation("Updating sample item: {Id}", command.Id);

        // Get existing item
        var sampleItem = await repository.GetByIdAsync(command.Id, cancellationToken);
        if (sampleItem == null)
        {
            return FauraResult<SampleItemDto>.Error(
                new FauraError("SAMPLE_ITEM_NOT_FOUND", $"Sample item with ID '{command.Id}' not found"));
        }

        // Check if new name already exists (excluding current item)
        if (!string.IsNullOrWhiteSpace(command.Name) && command.Name != sampleItem.Name)
        {
            var nameExists = await repository.ExistsByNameAsync(command.Name, command.Id, cancellationToken);
            if (nameExists)
            {
                return FauraResult<SampleItemDto>.Error(
                    new FauraError("SAMPLE_ITEM_NAME_EXISTS", $"Sample item with name '{command.Name}' already exists"));
            }
        }

        try
        {
            // Update entity properties
            sampleItem.Update(command.Name, command.Description);
            sampleItem.ChangeCategory(command.Category);
            sampleItem.ChangeStatus(command.Status);

            if (command.Tags != null)
            {
                sampleItem.SetTags(command.Tags);
            }

            // Save changes
            await repository.UpdateAsync(sampleItem, cancellationToken);

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

            logger.LogFauraInformation("Sample item updated successfully: {Id}", command.Id);

            return FauraResult<SampleItemDto>.Success(dto);
        }
        catch (ArgumentException ex)
        {
            logger.LogFauraError(ex, "Validation error updating sample item");
            return FauraResult<SampleItemDto>.Error(
                new FauraError("VALIDATION_ERROR", ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogFauraError(ex, "Error updating sample item: {Id}", command.Id);
            return FauraResult<SampleItemDto>.Error(
                new FauraError("SAMPLE_ITEM_UPDATE_ERROR", "An error occurred while updating the sample item"));
        }
    }
}
