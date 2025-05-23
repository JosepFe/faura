﻿namespace Faura.Infrastructure.Logger.Outputs.File;
public class LogFileOptions
{
    public const string SectionName = "Logging:Outputs:File";

    public bool Enable { get; set; } = false;
    public string LogFile { get; set; } = string.Empty;
    public bool? EnableDate { get; set; } = false;
}
