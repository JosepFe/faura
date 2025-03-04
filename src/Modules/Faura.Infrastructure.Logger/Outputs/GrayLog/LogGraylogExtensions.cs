namespace Faura.Infrastructure.Logger.Outputs.GrayLog;

using Serilog.Sinks.Graylog;
using Serilog;
using Microsoft.Extensions.Configuration;
using Serilog.Core;
using Serilog.Sinks.Graylog.Core.Transport;

public static class LogGrayLogExtensions
{
    public static LoggerConfiguration ConfigureLogGraylog(this LoggerConfiguration loggerConfiguration, IConfiguration configuration, List<ILogEventEnricher> logEnrichers = null)
    {
        var graylogOptions = configuration.GetSection(LogGraylogOptions.SectionName).Get<LogGraylogOptions>();

        if (graylogOptions?.Enable != true) return loggerConfiguration;

        var graylogSinkConfig = new GraylogSinkOptions
        {
            HostnameOrAddress = graylogOptions.GrayLogHost,
            TransportType = GetGraylogTransportTypeFromString(graylogOptions.GrayLogProtocol),
            HostnameOverride = graylogOptions.GrayLogHost,
            Port = graylogOptions.GrayLogPort,
            MaxMessageSizeInUdp = graylogOptions.MaxUdpMessageSize,
            Facility = "FauraApp"
        };

        return loggerConfiguration
            .WriteTo.Graylog(graylogSinkConfig);
    }

    private static TransportType GetGraylogTransportTypeFromString(string transportType)
    {
        if (transportType == null) return TransportType.Udp;
        return transportType.ToLower() switch
        {
            "tcp" => TransportType.Tcp,
            "udp" => TransportType.Udp,
            "http" => TransportType.Http,
            _ => TransportType.Udp
        };
    }
}
