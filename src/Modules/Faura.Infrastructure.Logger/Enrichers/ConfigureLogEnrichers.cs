namespace Faura.Infrastructure.Logger.Enrichers;

using Microsoft.Extensions.Configuration;
using Serilog;
using Faura.Infrastructure.Logger.Options;

internal static class LogEnrichersExtensions
{
    public static LoggerConfiguration ConfigureLogEnrichers(this LoggerConfiguration loggerConfiguration, IConfiguration configuration)
    {
        var loggingOptions = configuration.GetSection(LoggingOptions.SectionName).Get<LoggingOptions>();

        return loggerConfiguration
            .Enrich.WithProperty("ApplicationName", loggingOptions.ApplicationName)
            .Enrich.With(new CorrelationIdEnricher());
    }
}
