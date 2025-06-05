namespace Faura.Infrastructure.IntegrationTesting.Options;

public class TestContainerOptions
{
    public ContainerOptions Mongo { get; set; } = new();
    public ContainerOptions Redis { get; set; } = new();
    public ContainerOptions Postgres { get; set; } = new();
    public ContainerOptions SqlServer { get; set; } = new();
}
