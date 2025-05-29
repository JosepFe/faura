namespace Faura.Infrastructure.IntegrationTesting.Options;

public class ContainerOptions
{
    public string Image { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Database { get; set; } = string.Empty;
}
