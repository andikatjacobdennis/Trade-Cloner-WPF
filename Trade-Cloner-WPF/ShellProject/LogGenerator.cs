namespace ShellProject
{
    public class LogGenerator
    {
        private Random random = new Random();

        public string GenerateParentLog()
        {
            // Simulate a log from Kite Connect API for parent
            return $"Parent Log [{DateTime.Now}]: {GetRandomKiteConnectMessage()}";
        }

        public string GenerateChildLog()
        {
            // Simulate a log from Kite Connect API for child
            return $"Child Log [{DateTime.Now}]: {GetRandomKiteConnectMessage()}";
        }

        private string GetRandomKiteConnectMessage()
        {
            string[] messages = { "Received order update.", "Connection lost, retrying...", "Order executed successfully.", "API key expired." };
            int index = random.Next(messages.Length);
            return messages[index];
        }
    }
}
