namespace Faura.Infrastructure.Logger.Outputs.Seq;

using Serilog;
using Microsoft.Extensions.Configuration;
using Serilog.Core;

public static class LogSeqExtensions
{
    public static LoggerConfiguration ConfigureLogSeq(this LoggerConfiguration loggerConfiguration, IConfiguration configuration, List<ILogEventEnricher>? logEnrichers = null)
    {
        var seqOptions = configuration.GetSection(LogSeqOptions.SectionName).Get<LogSeqOptions>() ?? new LogSeqOptions();

        if (!seqOptions.Enable) return loggerConfiguration;
        return loggerConfiguration
            .WriteTo.Seq(serverUrl: seqOptions.ServerUrl!, apiKey: seqOptions.ApiKey);
    }
}
