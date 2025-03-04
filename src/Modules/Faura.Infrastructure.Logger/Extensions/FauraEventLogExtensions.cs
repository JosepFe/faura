namespace Faura.Infrastructure.Logger.Extensions;

using Microsoft.Extensions.Logging;

public static class FauraEventLogExtensions
{
    public static void LogFauraDebug(this ILogger logger, string message, params string?[] args) =>
        logger.Log(LogLevel.Debug, message, args);

    public static void LogFauraInformation(this ILogger logger, string message, params string?[] args) =>
        logger.Log(LogLevel.Information, message, args);

    public static void LogFauraWarning(this ILogger logger, Exception? exception, string message, params string?[] args) =>
        logger.Log(LogLevel.Warning, exception, message, args);

    public static void LogFauraError(this ILogger logger, Exception? exception, string message, params string?[] args) =>
        logger.Log(LogLevel.Error, exception, message, args);

    public static void LogFauraCritical(this ILogger logger, Exception? exception, string message, params string?[] args) =>
        logger.Log(LogLevel.Critical, exception, message, args);
}
