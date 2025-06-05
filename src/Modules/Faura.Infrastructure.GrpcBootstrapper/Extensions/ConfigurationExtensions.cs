using Faura.Configurations;
using Microsoft.Extensions.Configuration;

namespace Faura.Infrastructure.GrpcBootstrapper.Extensions;

public static class ConfigurationExtensions
{
    public static void ConfigureUserSecrets<T>(this IConfigurationBuilder configBuilder)
        where T : class
    {
        if (FauraEnvironment.IsLocal || FauraEnvironment.IsDevelopment)
        {
            configBuilder.AddUserSecrets<T>(optional: true);
        }
    }
}
