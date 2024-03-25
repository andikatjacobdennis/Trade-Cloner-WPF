using KiteConnect;
using ShellProject.CommonServices.Logging;

namespace ShellProject.CommonServices.TradeManagement
{
    public class TradeManager
    {
        private readonly FileLogger Logger;
        private readonly Ticker ticker;
        private readonly Kite kite;

        private readonly string MyAPIKey;
        private readonly string MySecret;
        private string MyAccessToken;

        public TradeManager(FileLogger logger, string apiKey, string accessToken, string secret)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            MyAPIKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            MySecret = secret ?? throw new ArgumentNullException(nameof(secret));
            MyAccessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));

            // Initialize logger with root folder and prefix
            logger = new FileLogger("Logs", "TradeManager");

            // Initialize Kite and Ticker
            kite = new Kite(MyAPIKey, Debug: false);
            kite.SetSessionExpiryHook(OnTokenExpire);

            // Initialize session
            InitSession();

            // Initialize Ticker
            ticker = new Ticker(MyAPIKey, MyAccessToken);
            ticker.OnTick += OnTick;
            ticker.OnReconnect += OnReconnect;
            ticker.OnNoReconnect += OnNoReconnect;
            ticker.OnError += OnError;
            ticker.OnClose += OnClose;
            ticker.OnConnect += OnConnect;
            ticker.OnOrderUpdate += OnOrderUpdate;
            ticker.EnableReconnect(Interval: 5, Retries: 50);
            ticker.Connect();
        }

        private void InitSession()
        {
            try
            {
                string requestToken = "SampleRequestToken"; // Placeholder for actual token retrieval
                User user = kite.GenerateSession(requestToken, MySecret);

                MyAccessToken = user.AccessToken;

                // Log session initialization
                Logger.Log(LogLevel.Info, "Session Initialized", $"Access Token: {MyAccessToken}");
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevel.Error, "Session Initialization Error", ex.Message, ex);
            }
        }

        private void OnTokenExpire()
        {
            Logger.Log(LogLevel.Warning, "Token Expired", "Need to login again");
        }

        private void OnConnect()
        {
            Logger.Log(LogLevel.Info, "Ticker Connected", "Ticker connected successfully");
        }

        private void OnClose()
        {
            Logger.Log(LogLevel.Info, "Ticker Closed", "Ticker connection closed");
        }

        private void OnError(string Message)
        {
            Logger.Log(LogLevel.Error, "Ticker Error", Message);
        }

        private void OnNoReconnect()
        {
            Logger.Log(LogLevel.Warning, "Ticker Reconnect Failed", "Not reconnecting to ticker");
        }

        private void OnReconnect()
        {
            Logger.Log(LogLevel.Info, "Ticker Reconnecting", "Reconnecting to ticker");
        }

        private void OnTick(Tick TickData)
        {
            Logger.Log(LogLevel.Info, "Tick Data Received", Utils.JsonSerialize(TickData));
        }

        private void OnOrderUpdate(Order OrderData)
        {
            Logger.Log(LogLevel.Info, "Order Update", Utils.JsonSerialize(OrderData));
        }
    }
}