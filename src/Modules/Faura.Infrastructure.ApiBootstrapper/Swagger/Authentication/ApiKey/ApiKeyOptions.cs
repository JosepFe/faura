using Microsoft.OpenApi.Models;

namespace Faura.Infrastructure.ApiBootstrapper.Swagger.Authentication.ApiKey;

public class ApiKeyOptions
{
    public const string SectionName = "Swagger:Authentication:ApiKey";
    public bool Enable { get; set; } = false;
    public string Name { get; set; } = "X-API-Key";
    public ParameterLocation In { get; set; } = ParameterLocation.Header; // Header o Query
}
