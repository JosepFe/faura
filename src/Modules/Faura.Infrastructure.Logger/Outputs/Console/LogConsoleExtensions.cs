namespace Faura.Infrastructure.Logger.Outputs.Console;

using Microsoft.Extensions.Configuration;
using Serilog.Core;
using Serilog;

public static class LogConsoleExtensions
{
    public const string BaseLogTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}";

    public static LoggerConfiguration ConfigureLogConsole(this LoggerConfiguration loggerConfiguration, IConfiguration configuration, List<ILogEventEnricher> logEnrichers = null)
    {
        var consoleOptions = configuration.GetSection(LogConsoleOptions.SectionName).Get<LogConsoleOptions>() ?? new LogConsoleOptions();

        if (!consoleOptions.Enable) return loggerConfiguration;

        return loggerConfiguration
            .WriteTo.Console(outputTemplate: consoleOptions.LogTemplate ?? BaseLogTemplate);
    }
}
