using System.IO;
using Newtonsoft.Json;
using ShellProject.CommonServices.TradeManagement;

namespace ShellProject.CommonServices.Logging
{
    public class JsonFileReader
    {
        private readonly FileLogger logger;

        public JsonFileReader(FileLogger logger)
        {
            this.logger = logger;
        }

        public TradeData? ReadDataFromJsonFile(string filePath)
        {
            TradeData? tradeData = null;
            try
            {
                if (File.Exists(filePath))
                {
                    string jsonData = File.ReadAllText(filePath);

                    // Deserialize JSON data into custom classes or structures
                    tradeData = JsonConvert.DeserializeObject<TradeData>(jsonData);
                    return tradeData;
                }
                else
                {
                    logger.Log(LogLevel.Error, "Log file error", "JSON file not found at the specified path.");
                }
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, "Log file error details", ex.Message, ex);
            }
            return tradeData;
        }
    }
}
