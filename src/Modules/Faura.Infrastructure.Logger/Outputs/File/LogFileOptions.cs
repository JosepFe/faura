namespace Faura.Infrastructure.Logger.Outputs.File;
public class LogFileOptions
{
    public const string SectionName = "Logging:Outputs:File";

    public bool Enable { get; set; }
    public string LogFile { get; set; }
    public bool? EnableDate { get; set; }
}
