namespace Faura.Infrastructure.Common.Utils;

using System.Reflection;

public static class FileUtils
{
    private static string? applicationDirectory;

    public static List<string> SearchFiles(string pattern, string baseDirectory = "")
    {
        string directory = ResolveDirectory(baseDirectory);
        return Directory.GetFiles(directory, pattern, SearchOption.AllDirectories).ToList();
    }

    public static string? GetFullFilePath(string fileName, string baseDirectory = "")
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return string.Empty;

        if (File.Exists(fileName))
            return Path.GetFullPath(fileName);

        string directory = ResolveDirectory(baseDirectory);
        return Directory
            .GetFiles(directory, fileName, SearchOption.AllDirectories)
            .FirstOrDefault();
    }

    public static string? GetApplicationDirectory()
        => applicationDirectory ??= Path.GetDirectoryName(
            Assembly.GetEntryAssembly()?.Location);

    private static string ResolveDirectory(string baseDirectory)
        => !string.IsNullOrWhiteSpace(baseDirectory) && Directory.Exists(baseDirectory)
            ? baseDirectory
            : Directory.GetCurrentDirectory();
}
