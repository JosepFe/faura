namespace Faura.Infrastructure.JWT.Options;

public class JwtOptions
{
    public const string SectionName = "JWT";

    public string? ValidIssuer {  get; set; }
    public string? Audience { get; set; }
    public string? MetadataAddress { get; set; }
    public string? AutorizationUrl { get; set; }
}
