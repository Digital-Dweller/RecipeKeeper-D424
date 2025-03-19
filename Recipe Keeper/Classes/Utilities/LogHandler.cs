using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_Keeper.Classes.Utilities
{
    public class LogHandler
    {
        private readonly string filePath;

        public LogHandler()
        {
            string directory = FileSystem.AppDataDirectory;
            filePath = Path.Combine(directory, "RecipeKeeperLog.txt");
        }

        //Log an event to the log file.
        public async Task LogAsync(string message)
        {
            string logMessage = $"{DateTime.Now}: {message}{Environment.NewLine}";
            await File.AppendAllTextAsync(filePath, logMessage);
        }

        //Print the log content to the screen.
        public async Task<string> ReadLogsAsync()
        {
            if (File.Exists(filePath))
            { return await File.ReadAllTextAsync(filePath); }
            else { return "No logs found."; }            
        }
    }
}
