namespace Faura.Infrastructure.Logger.Outputs.SqLiteDb;

public class LogSqLiteDbOptions
{
    public const string SectionName = "Logging:Outputs:SqLiteDb";

    public bool Enable { get; set; } = false;
    public string LogSqLiteFile { get; set; }
}
