namespace Faura.Infrastructure.Logger.Outputs.GrayLog;

using Serilog;
using Microsoft.Extensions.Configuration;
using Serilog.Core;

public static class LogSeqExtensions
{
    public static LoggerConfiguration ConfigureLogSeq(this LoggerConfiguration loggerConfiguration, IConfiguration configuration, List<ILogEventEnricher> logEnrichers = null)
    {
        var seqOptions = configuration.GetSection(LogSeqOptions.SectionName).Get<LogSeqOptions>();

        if (!seqOptions.Enable) return loggerConfiguration;
        return loggerConfiguration
            .WriteTo.Seq(serverUrl: seqOptions.ServerUrl, apiKey: seqOptions.ApiKey);
    }
}
