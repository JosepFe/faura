namespace Faura.Infrastructure.Logger.Outputs.Seq;

public class LogSeqOptions
{
    public const string SectionName = "Logging:Outputs:Seq";

    public bool Enable { get; set; } = false;
    public string? ServerUrl { get; set; }
    public string? ApplicationName { get; set; }
    public string? ApiKey { get; set; }
}
