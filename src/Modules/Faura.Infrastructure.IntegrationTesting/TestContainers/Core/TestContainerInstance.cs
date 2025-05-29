using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace Faura.Infrastructure.IntegrationTesting.TestContainers.Core;

public class TestContainerInstance<T> where T : ITestContainerConfiguration
{
    private readonly T _config;
    private readonly IContainer _container;

    public string ConnectionString { get; private set; } = string.Empty;

    public TestContainerInstance(T config)
    {
        _config = config;

        var builder = new ContainerBuilder()
            .WithImage(_config.Image)
            .WithName($"testcontainer-{Guid.NewGuid():N}")
            .WithCleanUp(true)
            .WithPortBinding(_config.Port, true);

        foreach (var kvp in _config.GetEnvironmentVariables())
        {
            builder = builder.WithEnvironment(kvp.Key, kvp.Value);
        }

        _container = builder.Build();
    }

    public async Task StartAsync()
    {
        await _container.StartAsync();
        var mappedPort = _container.GetMappedPublicPort(_config.Port);
        ConnectionString = _config.BuildConnectionString(mappedPort);
    }

    public Task StopAsync() => _container.StopAsync();
}
