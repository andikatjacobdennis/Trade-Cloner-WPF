using System.IO;
using Newtonsoft.Json;

public enum LogLevel
{
    Trace,
    Debug,
    Info,
    Warning,
    Error,
    Fatal
}

public class Log
{
    public Log()
    {
            
    }
    public Log(LogLevel level, string summary, string description)
    {
        Level = level;
        Summary = summary;
        Description = description;
        DateTime = DateTime.Now;
    }

    public DateTime DateTime { get; set; }
    public LogLevel Level { get; set; }
    public string MachineName { get; set; }
    public string UserName { get; set; }
    public string Summary { get; set; }
    public string Description { get; set; }
    public Exception Exception { get; set; }
    public string CallerMemberName { get; set; }
    public int CallerLineNumber { get; set; }
    public string CallerFilePath { get; set; }
}

public class FileLogger
{
    private readonly string _rootFolder;
    private readonly string _prefix;
    private bool _isEnabled;
    private readonly List<Log> _logs;
    private string logFilePath;

    public FileLogger(string rootFolder, string prefix)
    {
        _rootFolder = rootFolder ?? throw new ArgumentNullException(nameof(rootFolder));
        _prefix = prefix ?? throw new ArgumentNullException(nameof(prefix));
        _isEnabled = true;
        _logs = new List<Log>();
        string logFileName = $"{_prefix}_{DateTime.Now:yyyyMMdd_HHmmss}.json";
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
    }

    public void Log(LogLevel level, string summary, string description, Exception exception = null,
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

