using System.IO;
using Newtonsoft.Json;

namespace ShellProject.CommonServices.Logging
{
    public class FileLogger
    {
        private readonly string _rootFolder;
        private readonly string _prefix;
        private readonly bool _isEnabled;
        private readonly List<Log> _logs;
        private readonly string logFilePath;

        public delegate void LogInsertedEventHandler(Log log);
        public event LogInsertedEventHandler LogInserted;

        public FileLogger(string rootFolder, string prefix)
        {
            _rootFolder = rootFolder ?? throw new ArgumentNullException(nameof(rootFolder));
            _prefix = prefix ?? throw new ArgumentNullException(nameof(prefix));
            _isEnabled = true;
            _logs = new List<Log>();
            string logFileName = $"{_prefix}_{DateTime.Now.ToString(AppConstants.DateTimeFormatForFile)}.json";
            logFilePath = Path.Combine(_rootFolder, logFileName);

            if (!Directory.Exists(_rootFolder))
            {
                Directory.CreateDirectory(_rootFolder);
            }
        }

        private void WriteLog(Log log, string logFilePath)
        {
            string logJson = JsonConvert.SerializeObject(log, Formatting.Indented);

            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine(logJson);
            }

            // Trigger the LogInserted event
            LogInserted?.Invoke(log);
        }

        public void Log(LogLevel level, string summary, string description, Exception? exception = null,
            [System.Runtime.CompilerServices.CallerMemberName] string membName = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string filePath = "")
        {
            if (!_isEnabled)
                return;

            try
            {
                var logEntry = new Log
                {
                    DateTime = DateTime.Now,
                    Level = level,
                    MachineName = Environment.MachineName,
                    UserName = Environment.UserName,
                    Summary = summary,
                    Description = description,
                    Exception = exception,
                    CallerMemberName = membName,
                    CallerLineNumber = lineNumber,
                    CallerFilePath = filePath
                };

                WriteLog(logEntry, logFilePath);

                _logs.Add(logEntry);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine($"Error logging: {ex.Message}");
            }
        }
    }
}