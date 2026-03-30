namespace Template.Sample.Application.Handlers;

using Faura.Infrastructure.Logger.Extensions;
using Faura.Infrastructure.Result;
using Microsoft.Extensions.Logging;
using Template.Sample.Application.Commands;
using Template.Sample.Domain.Repositories;
using Template.Shared.Application.SimpleMediator;

/// <summary>
/// Handler for deleting a sample item.
/// </summary>
/// <param name="repository">The sample item repository.</param>
/// <param name="logger">The logger.</param>
public class DeleteSampleItemHandler(
    ISampleItemRepository repository,
    ILogger<DeleteSampleItemHandler> logger) : IRequestHandler<DeleteSampleItemCommand, FauraResult<bool>>
{

    /// <inheritdoc/>
    public async Task<FauraResult<bool>> Handle(
        DeleteSampleItemCommand command,
        CancellationToken cancellationToken = default)
    {
        logger.LogFauraInformation("Deleting sample item: {Id}", command.Id);

        // Check if item exists
        var sampleItem = await repository.GetByIdAsync(command.Id, cancellationToken);
        if (sampleItem == null)
        {
            return FauraResult<bool>.Error(
                new FauraError("SAMPLE_ITEM_NOT_FOUND", $"Sample item with ID '{command.Id}' not found"));
        }

        try
        {
            await repository.DeleteAsync(command.Id, cancellationToken);

            logger.LogFauraInformation("Sample item deleted successfully: {Id}", command.Id);

            return FauraResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            logger.LogFauraError(ex, "Error deleting sample item: {Id}", command.Id);
            return FauraResult<bool>.Error(
                new FauraError("SAMPLE_ITEM_DELETE_ERROR", "An error occurred while deleting the sample item"));
        }
    }
}
