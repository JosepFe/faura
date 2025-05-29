using Faura.Infrastructure.IntegrationTesting.Options;
using Faura.Infrastructure.IntegrationTesting.TestContainers.Constants;
using Faura.Infrastructure.IntegrationTesting.TestContainers.Core;

namespace Faura.Infrastructure.IntegrationTesting.TestContainers.Configurations;

public class SqlServerContainerConfiguration : ITestContainerConfiguration
{
    private readonly ContainerOptions _options;

    public SqlServerContainerConfiguration(ContainerOptions options)
    {
        _options = options;
    }

    public string Image => string.IsNullOrWhiteSpace(_options.Image)
        ? ContainerDefaultsConstants.Images.SqlServer
        : _options.Image;

    public int Port => _options.Port != 0
        ? _options.Port
        : ContainerDefaultsConstants.Ports.SqlServer;

    public string Username => _options.Username ?? "sa";
    public string Password => _options.Password ?? "Your_strong_password123!";
    public string Database => _options.Database ?? "TestDb";

    public Dictionary<string, string> GetEnvironmentVariables() => new()
    {
        ["ACCEPT_EULA"] = "Y",
        ["SA_PASSWORD"] = Password
    };

    public string BuildConnectionString(int mappedPort) =>
        $"Server=localhost,{mappedPort};Database={Database};User Id={Username};Password={Password};TrustServerCertificate=True";
}
