namespace Faura.Infrastructure.Messaging.Extensions;

using Faura.Infrastructure.Common.Utils;
using Faura.Infrastructure.Messaging.Options;
using Faura.Infrastructure.Messaging.Services;
using Faura.Infrastructure.Messaging.Services.Producer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPulsarMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        var pulsarOptions = services.GetTypedOptions<PulsarOptions>(configuration, PulsarOptions.SectionName);

        // Configuración del cliente Pulsar
        services.AddPulsarClient(options =>
        {
            options.ServiceUrl = new Uri(pulsarOptions!.ServiceUrl);
        });

        services.AddSingleton<IPulsarMessagingService, PulsarMessagingService>();
        services.AddScoped<IPulsarProducerMessageService, PulsarProducerMessageService>();
        return services;
    }
} 