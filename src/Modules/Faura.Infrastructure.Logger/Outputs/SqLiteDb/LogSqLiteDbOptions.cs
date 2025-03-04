namespace Faura.Infrastructure.Logger.Outputs.SqLiteDb;

public class LogSqLiteDbOptions
{
    public const string SectionName = "Logging:Outputs:SqLiteDb";

    public bool Enable { get; set; }
    public string LogSqLiteFile { get; set; }
}
