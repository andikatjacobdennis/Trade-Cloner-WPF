using System;
using System.IO;
using Newtonsoft.Json;
using static ShellProject.MainWindow;

namespace ShellProject
{
    public class JsonFileReader
    {
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
                    Console.WriteLine("JSON file not found at the specified path.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading JSON file: " + ex.Message);
            }
            return tradeData;
        }
    }
}
