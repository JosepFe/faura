namespace Faura.Infrastructure.IntegrationTesting.TestContainers.Configurations;

using Faura.Infrastructure.IntegrationTesting.Options;
using Faura.Infrastructure.IntegrationTesting.TestContainers.Core;

public class SqlServerContainerConfiguration : ITestContainerConfiguration
{
    private const string DefaultImage = "mcr.microsoft.com/mssql/server:2022-lts";
    private const int SqlServerInternalPort = 1433;
    private readonly ContainerOptions _options;

    public SqlServerContainerConfiguration(ContainerOptions options) => _options = options;

    public string Image =>
        string.IsNullOrWhiteSpace(_options.Image)
            ? DefaultImage
            : _options.Image;

    public int? Port =>_options.Port;

    public int InternalPort => SqlServerInternalPort;
    public string Username => _options.Username ?? "sa";
    public string Password => _options.Password ?? "Your_strong_password123!";
    public string Database => _options.Database ?? "TestDb";

    public Dictionary<string, string> GetEnvironmentVariables() =>
        new()
        {
            ["ACCEPT_EULA"] = "Y",
            ["SA_PASSWORD"] = Password,
            ["MSSQL_PID"] = "Developer",
        };

    public string BuildConnectionString(string host, int mappedPort) =>
        $"Server={host},{mappedPort};Database={Database};User Id={Username};Password={Password};TrustServerCertificate=True";
}
