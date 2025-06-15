namespace Faura.Infrastructure.Logger;

using Microsoft.Extensions.Hosting;
using Faura.Infrastructure.Logger.Outputs.File;
using Faura.Infrastructure.Logger.Outputs.GrayLog;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Settings.Configuration;
using Faura.Infrastructure.Logger.Outputs.Console;
using Faura.Infrastructure.Logger.Outputs.SqLiteDb;
using Faura.Infrastructure.Logger.Enrichers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Faura.Infrastructure.Logger.Options;
using Faura.Infrastructure.Logger.Outputs.Seq;

public static class LoggingConfiguration
{
    public static void SetupLogging(this IHostBuilder builder)
    {
        Action<HostBuilderContext, IServiceProvider, LoggerConfiguration> configureLogger = (context, _, loggerConfiguration) => loggerConfiguration.ConfigureFauraLogging(context.Configuration);

        builder.UseSerilog(configureLogger);
    }

    public static void SetupLogging(this IServiceCollection services, IConfiguration configuration)
    {
        var loggerConfiguration = new LoggerConfiguration();
        ConfigureFauraLogging(loggerConfiguration, configuration);

        Log.Logger = loggerConfiguration.CreateLogger();

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog();
        });
    }

    public static LoggerConfiguration ConfigureFauraLogging(
    this LoggerConfiguration loggerConfiguration,
    IConfiguration configuration,
    List<ILogEventEnricher>? logEnrichers = default)
    {
        loggerConfiguration
            .ReadFrom.Configuration(configuration, new ConfigurationReaderOptions
            {
                SectionName = LoggingOptions.SectionName,
            })
            .ConfigureLogEnrichers(configuration)
            .ConfigureLogConsole(configuration)
            .ConfigureLogFile(configuration)
            .ConfigureLogGraylog(configuration)
            .ConfigureLogSeq(configuration)
            .ConfigureLogSqLiteDb(configuration);

        return loggerConfiguration;
    }
}
