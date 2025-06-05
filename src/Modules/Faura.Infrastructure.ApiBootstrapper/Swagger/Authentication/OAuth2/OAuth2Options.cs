namespace Faura.Infrastructure.ApiBootstrapper.Swagger.Authentication.OAuth2;

public class OAuth2Options
{
    public const string SectionName = "Swagger:Authentication:OAuth2";

    public bool Enable { get; set; } = false;
    public string Name { get; set; }
    public string AuthenticationURL { get; set; }
    public Dictionary<string, string> Scopes { get; set; } =
        new() { { "openid", "openid" }, { "profile", "profile" } };
}
