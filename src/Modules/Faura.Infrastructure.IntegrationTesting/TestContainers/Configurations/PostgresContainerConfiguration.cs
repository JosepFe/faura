using Faura.Infrastructure.IntegrationTesting.Options;
using Faura.Infrastructure.IntegrationTesting.TestContainers.Constants;
using Faura.Infrastructure.IntegrationTesting.TestContainers.Core;

namespace Faura.Infrastructure.IntegrationTesting.TestContainers.Configurations;

public class PostgresContainerConfiguration : ITestContainerConfiguration
{
    private readonly ContainerOptions _options;

    public PostgresContainerConfiguration(ContainerOptions options)
    {
        _options = options;
    }

    public string Image =>
        string.IsNullOrWhiteSpace(_options.Image)
            ? ContainerDefaultsConstants.Images.Postgres
            : _options.Image;

    public int Port =>
        _options.Port != 0 ? _options.Port : ContainerDefaultsConstants.Ports.Postgres;

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

    public string BuildConnectionString(int mappedPort) =>
        $"Host=localhost;Port={mappedPort};Username={Username};Password={Password};Database={Database}";
}
