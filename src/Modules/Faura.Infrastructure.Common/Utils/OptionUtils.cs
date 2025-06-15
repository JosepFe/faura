namespace Faura.Infrastructure.Common.Utils;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

public static class OptionUtils
{
    public static T? GetTypedOptions<T>(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName)
        where T : class, new()
    {
        if (configuration == null || string.IsNullOrWhiteSpace(sectionName))
            return null;

        services.Configure<T>(configuration.GetSection(sectionName));
        using var serviceProvider = services.BuildServiceProvider();
        return serviceProvider.GetRequiredService<IOptions<T>>().Value;
    }
}
