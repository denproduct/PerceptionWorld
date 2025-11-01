using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerceptionWorld.Utils;


public static class LogInfo
{
    private static string LogPath = "Logs/debug.log";

    public static void Write(string message)
    {
        string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
        //File.AppendAllText(LogPath, logEntry + Environment.NewLine);
        Console.WriteLine(logEntry); // Для отладки в консоль
    }

    public static void Error(string message, Exception ex = null)
    {
        string errorMsg = $"ERROR: {message}";
        if (ex != null) errorMsg += $" | Exception: {ex.Message}";
        Write(errorMsg);
    }
}