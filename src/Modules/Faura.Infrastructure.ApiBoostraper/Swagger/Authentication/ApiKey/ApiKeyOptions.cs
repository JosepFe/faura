namespace Faura.Infrastructure.ApiBoostraper.Swagger.Authentication.ApiKey;

using Microsoft.OpenApi.Models;

public class ApiKeyOptions
{
    public const string SectionName = "Swagger:Authentication:ApiKey";
    public bool Enable { get; set; } = false;
    public string Name { get; set; } = "X-API-Key";
    public ParameterLocation In { get; set; } = ParameterLocation.Header; // Header o Query
}
