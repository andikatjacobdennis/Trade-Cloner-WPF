using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ShellProject
{
    public partial class MainWindow : Window
    {
        private FileLogger loggerApplication;
        private FileLogger loggerParent;
        private FileLogger loggerChild;
        JsonFileReader jsonFileReader = new JsonFileReader();

        public ObservableCollection<Log> ParentLogs { get; set; } = new ObservableCollection<Log>();
        public ObservableCollection<Log> ChildLogs { get; set; } = new ObservableCollection<Log>();
        // Create an instance of InternetOutageDetector
        InternetOutageDetector detector;

        public MainWindow()
        {
            InitializeComponent();

            // Initialize logger with root folder and prefix
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string rootPath = Path.Combine(desktopPath, "Logs_TradeClonewWPF");
            loggerApplication = new FileLogger(rootPath, "Application");
            loggerParent = new FileLogger(rootPath, "Parent");
            loggerChild = new FileLogger(rootPath, "Child");

            // Set the data context for the logs list boxes
            ParentLogsListBox.ItemsSource = ParentLogs;
            ChildLogsListBox.ItemsSource = ChildLogs;

            // Simulate adding logs every half second
            UpdateApplicationStartTime();
            LoginParentAccount();
            LoginChildAccount();
            StartLogging();
            DetectInternetOutage();
        }

        private void DetectInternetOutage()
        {
            // Create an instance of InternetOutageDetector
            detector = new InternetOutageDetector();
            detector.InternetConnectionChanged += HandleInternetConnectionChanged;
            detector.Start();
            LastOutageLabel.Content = detector.GetLastOutage();
            TotalOutagesLabel.Content = detector.GetTotalOutages();
        }

        // Event handler method for InternetConnectionChanged event
        void HandleInternetConnectionChanged(object sender, InternetConnectionChangedEventArgs e)
        {
            if (e.IsInternetAvailable)
            {
                loggerApplication.Log(LogLevel.Info, "Is Internet Available", "Internet Available");
            }
            else
            {
                loggerApplication.Log(LogLevel.Warning, "Is Internet Available", "Internet Not Available");
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    LastOutageLabel.Content = detector.GetLastOutage();
                    TotalOutagesLabel.Content = detector.GetTotalOutages();
                }));
            }
        }

        private void UpdateApplicationStartTime()
        {
            StartTimeLabel.Content = DateTime.Now.ToString();
            loggerApplication.Log(LogLevel.Info, "Application Started", "The application has initiated successfully.");

        }

        private async void StartLogging()
        {
            while (true)
            {
                // Add logs to the collections

                AddDataToLog(new Log(LogLevel.Info, "Parent", "ParentDescription"), ParentLogs, ParentLogsScrollViewer);
                AddDataToLog(new Log(LogLevel.Info, "Child", "ChildDescription"), ChildLogs, ChildLogsScrollViewer);

                AddDataToLog(new Log(LogLevel.Warning, "Parent", "ParentDescription"), ParentLogs, ParentLogsScrollViewer);
                AddDataToLog(new Log(LogLevel.Warning, "Child", "ChildDescription"), ChildLogs, ChildLogsScrollViewer);

                AddDataToLog(new Log(LogLevel.Error, "Parent", "ParentDescription"), ParentLogs, ParentLogsScrollViewer);
                AddDataToLog(new Log(LogLevel.Error, "Child", "ChildDescription"), ChildLogs, ChildLogsScrollViewer);

                AddDataToLog(new Log(LogLevel.Fatal, "Parent", "ParentDescription"), ParentLogs, ParentLogsScrollViewer);
                AddDataToLog(new Log(LogLevel.Fatal, "Child", "ChildDescription"), ChildLogs, ChildLogsScrollViewer);

                // Wait for half a second
                await Task.Delay(500);
            }
        }

        // Example method to add data to the log and scroll to bottom if needed
        private void AddDataToLog(Log data, ObservableCollection<Log> logCollection, ScrollViewer scrollViewer)
        {
            logCollection.Add(data);
            // Check if the scrollbar is at the bottom
            bool isAtBottom = scrollViewer.VerticalOffset + scrollViewer.ViewportHeight >= scrollViewer.ExtentHeight;

            // If already at bottom, scroll to bottom
            if (isAtBottom)
            {
                scrollViewer.ScrollToEnd();
            }
        }

        private void LoginParentAccount()
        {
            var tradeData = jsonFileReader.ReadDataFromJsonFile(Constants.jsonDataFilePath); // Call the method to read data from JSON file for parent account

            // Simulate parent account login
            ParentLoginLabel.Content = "Logged In " + DateTime.Now; // Update the status indicator
        }

        private void LoginChildAccount()
        {
            var tradeData = jsonFileReader.ReadDataFromJsonFile(Constants.jsonDataFilePath); // Call the method to read data from JSON file for child account

            // Simulate parent account login
            ChildLoginLabel.Content = "Logged In " + DateTime.Now; // Update the status indicator
        }

        private void ViewDetailInternetOutages_Click(object sender, RoutedEventArgs e)
        {
            ShowInternetDisruptions showInternetDisruptions = new ShowInternetDisruptions(detector);
            showInternetDisruptions.ShowDialog();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Ask for password when closing the application
            if (!VerifyPassword())
            {
                e.Cancel = true; // Cancel the closing event if the password is incorrect
            }
        }

        private bool VerifyPassword()
        {
            // Create and show the custom password prompt window
            var passwordPromptWindow = new PasswordPromptWindow();
            bool? result = passwordPromptWindow.ShowDialog();

            // Check if OK button was clicked and the entered password is correct
            if (result == true && passwordPromptWindow.EnteredPassword == "123")
            {
                return true; // Allow closing the application
            }
            else
            {
                return false; // Cancel closing the application
            }
        }
    }
}
