namespace Faura.Infrastructure.IntegrationTesting.TestContainers.Core;

public interface ITestContainerConfiguration
{
    string Image { get; }
    int Port { get; }
    string Username { get; }
    string Password { get; }
    string Database { get; }

    Dictionary<string, string> GetEnvironmentVariables();
    string BuildConnectionString(int mappedPort);
}