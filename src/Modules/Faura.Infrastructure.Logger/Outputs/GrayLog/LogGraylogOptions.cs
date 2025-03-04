namespace Faura.Infrastructure.Logger.Outputs.GrayLog;

public class LogGraylogOptions
{
    public const string SectionName = "Logging:Outputs:Graylog";

    public bool Enable {  get; set; }
    public string GrayLogHost { get; set; }
    public int GrayLogPort { get; set; }
    public string GrayLogProtocol { get; set; }
    public int MaxUdpMessageSize { get; set; }
}
