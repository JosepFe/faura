namespace Faura.Infrastructure.ApiBootstrapper.Swagger.Authentication.Bearer;

public class BearerOptions
{
    public const string SectionName = "Swagger:Authentication:Bearer";

    public bool Enable { get; set; } = false;
    public string Name { get; set; } = "Bearer";
    public string Scheme { get; set; } = "bearer";
}
