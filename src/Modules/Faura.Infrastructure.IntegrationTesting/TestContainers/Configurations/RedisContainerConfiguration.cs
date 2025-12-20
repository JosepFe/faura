using Faura.Infrastructure.IntegrationTesting.Options;
using Faura.Infrastructure.IntegrationTesting.TestContainers.Core;

namespace Faura.Infrastructure.IntegrationTesting.TestContainers.Configurations;

public class RedisContainerConfiguration : ITestContainerConfiguration
{
    private const string DefaultImage = "redis:7-alpine";
    private const int RedisInternalPort = 6379;

    private readonly ContainerOptions _options;

    public RedisContainerConfiguration(ContainerOptions options)
    {
        _options = options;
    }

    public string Image =>
        string.IsNullOrWhiteSpace(_options.Image)
            ? DefaultImage
            : _options.Image;

    public int? Port => _options.Port;

    public int InternalPort => RedisInternalPort;

    public string Username => _options.Username ?? string.Empty;

    public string Password => _options.Password ?? string.Empty;

    public string Database => _options.Database ?? "0";

    public Dictionary<string, string> GetEnvironmentVariables()
    {
        var env = new Dictionary<string, string>();

        if (!string.IsNullOrWhiteSpace(Password))
        {
            env["REDIS_PASSWORD"] = Password;
        }

        return env;
    }

    public string BuildConnectionString(string host, int mappedPort)
    {
        var connectionString = $"{host}:{mappedPort}";

        if (!string.IsNullOrWhiteSpace(Password))
        {
            connectionString += $",password={Password}";
        }

        if (!string.IsNullOrWhiteSpace(Database) && Database != "0")
        {
            connectionString += $",defaultDatabase={Database}";
        }

        return connectionString;
    }
}