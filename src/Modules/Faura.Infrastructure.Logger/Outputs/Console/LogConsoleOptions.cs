﻿namespace Faura.Infrastructure.Logger.Outputs.Console;

public class LogConsoleOptions
{
    public const string SectionName = "Logging:Outputs:Console";

    public bool Enable { get; set; }
    public string LogTemplate { get; set; }
}
