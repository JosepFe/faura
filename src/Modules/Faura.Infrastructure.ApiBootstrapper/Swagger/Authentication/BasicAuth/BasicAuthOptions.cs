namespace Faura.Infrastructure.ApiBootstrapper.Swagger.Authentication.BasicAuth;

public class BasicAuthOptions
{
    public const string SectionName = "Swagger:Authentication:BasicAuth";
    public bool Enable { get; set; } = false;
    public string Name { get; set; } = "Basic";
}
