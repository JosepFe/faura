using Faura.Infrastructure.IntegrationTesting.Options;
using Faura.Infrastructure.IntegrationTesting.TestContainers.Constants;
using Faura.Infrastructure.IntegrationTesting.TestContainers.Core;

namespace Faura.Infrastructure.IntegrationTesting.TestContainers.Configurations;

public class RedisContainerConfiguration : ITestContainerConfiguration
{
    private readonly ContainerOptions _options;

    public RedisContainerConfiguration(ContainerOptions options)
    {
        _options = options;
    }

    public string Image =>
        string.IsNullOrWhiteSpace(_options.Image)
            ? ContainerDefaultsConstants.Images.Redis
            : _options.Image;

    public int Port => _options.Port != 0 ? _options.Port : ContainerDefaultsConstants.Ports.Redis;

    public string Username => _options.Username ?? string.Empty;
    public string Password => _options.Password ?? string.Empty;
    public string Database => _options.Database ?? string.Empty;

    public Dictionary<string, string> GetEnvironmentVariables() => new();

    public string BuildConnectionString(int mappedPort) => $"localhost:{mappedPort}";
}
