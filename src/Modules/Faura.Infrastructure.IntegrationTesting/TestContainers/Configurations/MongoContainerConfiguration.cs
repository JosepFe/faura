using Faura.Infrastructure.IntegrationTesting.Options;
using Faura.Infrastructure.IntegrationTesting.TestContainers.Core;

namespace Faura.Infrastructure.IntegrationTesting.TestContainers.Configurations;

public class MongoContainerConfiguration : ITestContainerConfiguration
{
    private const string DefaultImage = "mongo:7-jammy";
    private const int MongoInternalPort = 27017;

    private readonly ContainerOptions _options;

    public MongoContainerConfiguration(ContainerOptions options)
    {
        _options = options;
    }

    public string Image =>
        string.IsNullOrWhiteSpace(_options.Image)
            ? DefaultImage
            : _options.Image;

    public int? Port => _options.Port;

    public int InternalPort => MongoInternalPort;

    public string Username => _options.Username ?? string.Empty;

    public string Password => _options.Password ?? string.Empty;

    public string Database => _options.Database ?? "test";

    public Dictionary<string, string> GetEnvironmentVariables()
    {
        var env = new Dictionary<string, string>();

        if (!string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password))
        {
            env["MONGO_INITDB_ROOT_USERNAME"] = Username;
            env["MONGO_INITDB_ROOT_PASSWORD"] = Password;
        }

        if (!string.IsNullOrWhiteSpace(Database))
        {
            env["MONGO_INITDB_DATABASE"] = Database;
        }

        return env;
    }

    public string BuildConnectionString(string host, int mappedPort)
    {
        if (!string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password))
        {
            return $"mongodb://{Username}:{Password}@{host}:{mappedPort}/{Database}?authSource=admin";
        }

        return $"mongodb://{host}:{mappedPort}/{Database}";
    }
}