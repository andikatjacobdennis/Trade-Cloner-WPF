namespace ShellProject.CommonServices.Logging
{
    public class Log
    {
        public DateTime DateTime { get; set; }
        public LogLevel Level { get; set; }
        public string? MachineName { get; set; }
        public string? UserName { get; set; }
        public string? Summary { get; set; }
        public string? Description { get; set; }
        public Exception? Exception { get; set; }
        public string? CallerMemberName { get; set; }
        public int CallerLineNumber { get; set; }
        public string? CallerFilePath { get; set; }
    }
}