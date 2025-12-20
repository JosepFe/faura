namespace Faura.Infrastructure.Common.Models;
public enum FauraDeploymentMode
{
    Local,
    Test,
    Development,
    Integration,
    Staging,
    Production,
}

public static class FauraEnvironment
{
    private const string FauraEnvVariableName = "FAURA_ENVIRONMENT";
    private static FauraDeploymentMode? _currentMode;

    public static FauraDeploymentMode CurrentMode => _currentMode ??= DetectMode();

    public static bool IsProduction => CurrentMode == FauraDeploymentMode.Production;
    public static bool IsLocal => CurrentMode == FauraDeploymentMode.Local;
    public static bool IsTest => CurrentMode == FauraDeploymentMode.Test;
    public static bool IsDevelopment => CurrentMode == FauraDeploymentMode.Development;

    private static FauraDeploymentMode DetectMode()
    {
        var mode =
            Environment.GetEnvironmentVariable(FauraEnvVariableName)
            ?? nameof(FauraDeploymentMode.Staging);
        return mode switch
        {
            nameof(FauraDeploymentMode.Local) => FauraDeploymentMode.Local,
            nameof(FauraDeploymentMode.Test) => FauraDeploymentMode.Test,
            nameof(FauraDeploymentMode.Development) => FauraDeploymentMode.Development,
            nameof(FauraDeploymentMode.Integration) => FauraDeploymentMode.Integration,
            nameof(FauraDeploymentMode.Staging) => FauraDeploymentMode.Staging,
            nameof(FauraDeploymentMode.Production) => FauraDeploymentMode.Production,
            _ => FauraDeploymentMode.Local,
        };
    }
}
