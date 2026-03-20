using Microsoft.Extensions.Configuration;

using RecruProject.Core.Logger;

namespace RecruProject.Console.Logger;

public class ConsoleLogger(IConfiguration configuration) : ILogger
{
    public void LogInfo(string message)
    {
        var logLevel = configuration["LogLevel"];
        if (string.Equals(logLevel, "Error", StringComparison.OrdinalIgnoreCase))
            return;
        
        System.Console.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] [INFO] {message}");
    }

    public void LogError(string message, Exception ex)
    {
        System.Console.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] [ERROR] {message}.\nException message: {ex.Message}\nStackTrace: {ex.StackTrace}");
    }
}