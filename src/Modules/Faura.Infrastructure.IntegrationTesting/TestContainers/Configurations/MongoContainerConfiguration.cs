using Faura.Infrastructure.IntegrationTesting.Options;
using Faura.Infrastructure.IntegrationTesting.TestContainers.Constants;
using Faura.Infrastructure.IntegrationTesting.TestContainers.Core;

namespace Faura.Infrastructure.IntegrationTesting.TestContainers.Configurations;

public class MongoContainerConfiguration : ITestContainerConfiguration
{
    private readonly ContainerOptions _options;

    public MongoContainerConfiguration(ContainerOptions options)
    {
        _options = options;
    }

    public string Image =>
        string.IsNullOrWhiteSpace(_options.Image)
            ? ContainerDefaultsConstants.Images.Mongo
            : _options.Image;

    public int Port => _options.Port != 0 ? _options.Port : ContainerDefaultsConstants.Ports.Mongo;

    public string Username => _options.Username ?? string.Empty;
    public string Password => _options.Password ?? string.Empty;
    public string Database => _options.Database ?? "test";

    public Dictionary<string, string> GetEnvironmentVariables() => new();

    public string BuildConnectionString(int mappedPort) =>
        $"mongodb://localhost:{mappedPort}/{Database}";
}
