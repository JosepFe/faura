namespace Faura.Infrastructure.IntegrationTesting.TestContainers.Configurations;

using Faura.Infrastructure.IntegrationTesting.Options;
using Faura.Infrastructure.IntegrationTesting.TestContainers.Core;

public class PostgresContainerConfiguration : ITestContainerConfiguration
{
    private const string DefaultImage = "postgres:15-alpine";
    private const int PostgresInternalPort = 5432;

    private readonly ContainerOptions _options;

    public PostgresContainerConfiguration(ContainerOptions options)
    {
        _options = options;
    }

    public string Image =>
        string.IsNullOrWhiteSpace(_options.Image)
            ? DefaultImage
            : _options.Image;

    public int? Port => _options.Port;

    public int InternalPort => PostgresInternalPort;

    public string Username => _options.Username ?? "postgres";
    public string Password => _options.Password ?? "postgres";
    public string Database => _options.Database ?? "test";

    public Dictionary<string, string> GetEnvironmentVariables() =>
        new()
        {
            ["POSTGRES_USER"] = Username,
            ["POSTGRES_PASSWORD"] = Password,
            ["POSTGRES_DB"] = Database,
        };

    public string BuildConnectionString(string host, int mappedPort) =>
        $"Host={host};Port={mappedPort};Username={Username};Password={Password};Database={Database}";
}
