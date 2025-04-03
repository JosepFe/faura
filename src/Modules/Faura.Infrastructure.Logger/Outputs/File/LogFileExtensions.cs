namespace Faura.Infrastructure.Logger.Outputs.File;

using Faura.Infrastructure.Common.Utils;
using Microsoft.Extensions.Configuration;
using Serilog;

public static class LogFileExtensions
{
    public const string DefaultLogFileName = "log.txt";

    public static LoggerConfiguration ConfigureLogFile(this LoggerConfiguration loggerConfiguration, IConfiguration configuration)
    {
        var fileOptions = configuration.GetSection(LogFileOptions.SectionName).Get<LogFileOptions>() ?? new LogFileOptions();

        if (!fileOptions.Enable) return loggerConfiguration;

        return loggerConfiguration
            .WriteTo.File(GetLogFilePath(fileOptions.LogFile, DefaultLogFileName, fileOptions.EnableDate ?? true));
    }

    internal static string GetLogFilePath(string file, string defaultFileName, bool enableDate)
    {
        string appPath = FileUtils.GetApplicationDirectory() ?? string.Empty;
        string timestamp = DateTime.Now.ToString("yyyyMMdd");

        if (string.IsNullOrWhiteSpace(file) || string.IsNullOrWhiteSpace(Path.GetFileName(file)))
            file = defaultFileName;

        string directory = Path.GetDirectoryName(file) ?? appPath;
        string fileNameWithoutExt = Path.GetFileNameWithoutExtension(file);
        string extension = Path.GetExtension(file);

        string newFileName = enableDate
            ? $"{fileNameWithoutExt}_{timestamp}{extension}"
            : $"{fileNameWithoutExt}{extension}";

        string fullPath = Path.IsPathRooted(file)
            ? Path.Combine(directory, newFileName)
            : Path.Combine(appPath, newFileName);

        return Path.GetFullPath(fullPath);
    }
}
