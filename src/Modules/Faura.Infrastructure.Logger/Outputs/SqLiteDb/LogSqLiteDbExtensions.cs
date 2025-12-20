namespace Faura.Infrastructure.Logger.Outputs.SqLiteDb;

using Faura.Infrastructure.Common.Utils;
using Microsoft.Extensions.Configuration;
using Serilog;

public static class LogSqLiteDbExtensions
{
    public const string DefaultLogSqLiteFileName = "fauraLogDatabase.db";

    public static LoggerConfiguration ConfigureLogSqLiteDb(this LoggerConfiguration loggerConfiguration, IConfiguration configuration)
    {
        var sqLiteDbOptions = configuration.GetSection(LogSqLiteDbOptions.SectionName).Get<LogSqLiteDbOptions>() ?? new LogSqLiteDbOptions();

        if (!sqLiteDbOptions.Enable) return loggerConfiguration;

        return loggerConfiguration
            .WriteTo.SQLite(GetSqliteDbLogFilePath(sqLiteDbOptions.LogSqLiteFile!, DefaultLogSqLiteFileName));
    }

    internal static string GetSqliteDbLogFilePath(string file, string defaultFileName)
    {
        string appPath = FileUtils.GetApplicationDirectory() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(file) || string.IsNullOrWhiteSpace(Path.GetFileName(file)))
            return Path.Combine(appPath, defaultFileName);

        return Path.GetFullPath(Path.IsPathRooted(file) ? file : Path.Combine(appPath, file));
    }
}
