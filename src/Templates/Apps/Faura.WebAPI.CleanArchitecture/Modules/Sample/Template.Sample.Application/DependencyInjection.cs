namespace Template.Sample.Application;

using Faura.Infrastructure.Result;
using Microsoft.Extensions.DependencyInjection;
using Template.Sample.Application.Commands;
using Template.Sample.Application.DTOs;
using Template.Sample.Application.Handlers;
using Template.Sample.Application.Queries;
using Template.Shared.Application.SimpleMediator;
using Template.Shared.Domain.Models;

/// <summary>
/// Dependency injection configuration for Sample application layer.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds Sample application handlers to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        // Command Handlers
        services.AddScoped<IRequestHandler<CreateSampleItemCommand, FauraResult<DTOs.SampleItemDto>>, CreateSampleItemHandler>();
        services.AddScoped<IRequestHandler<UpdateSampleItemCommand, FauraResult<DTOs.SampleItemDto>>, UpdateSampleItemHandler>();
        services.AddScoped<IRequestHandler<DeleteSampleItemCommand, FauraResult<bool>>, DeleteSampleItemHandler>();

        // Query Handlers
        services.AddScoped<IRequestHandler<GetSampleItemByIdQuery, FauraResult<DTOs.SampleItemDto>>, GetSampleItemByIdHandler>();
        services.AddScoped<IRequestHandler<GetSampleItemsQuery, FauraResult<Shared.Domain.Models.PagedResult<DTOs.SampleItemDto>>>, GetSampleItemsHandler>();

        return services;
    }
}
