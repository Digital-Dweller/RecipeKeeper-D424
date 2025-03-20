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
        public async Task LogMessage(string message)
        {
            string logMessage = $"{DateTime.Now}: {message}{Environment.NewLine}";
            await File.AppendAllTextAsync(filePath, logMessage);
        }

        //Print the log content to the screen.
        public async Task ReadLogs()
        {
            if (File.Exists(filePath))
            {
                var logLines = await File.ReadAllLinesAsync(filePath);
                int lineNumber = 0;
                foreach (var line in logLines)
                { Console.WriteLine($"{++lineNumber}: " + line); }
            }
            else
            { Console.WriteLine("No logs found."); }
        }
    }
}
