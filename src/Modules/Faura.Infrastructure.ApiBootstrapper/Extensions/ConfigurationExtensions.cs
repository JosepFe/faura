namespace Faura.Infrastructure.ApiBootstrapper.Extensions;

using Faura.Infrastructure.Common.Models;
using Microsoft.Extensions.Configuration;

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
