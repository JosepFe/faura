namespace Faura.Infrastructure.IntegrationTesting.TestContainers.Core;

public interface ITestContainerConfiguration
{
    string Image { get; }
    int? Port { get; }
    int InternalPort { get; }
    string Username { get; }
    string Password { get; }
    string Database { get; }

    Dictionary<string, string> GetEnvironmentVariables();
    string BuildConnectionString(string host, int mappedPort);
}
