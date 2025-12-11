namespace Faura.Infrastructure.IntegrationTesting.Options;

public class TestContainerOptions
{
    public ContainerOptions SqlServer { get; set; } = new ();
    public ContainerOptions Postgres { get; set; } = new();
}
