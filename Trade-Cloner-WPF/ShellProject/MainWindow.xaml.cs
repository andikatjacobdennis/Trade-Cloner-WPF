using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace ShellProject
{
    public partial class MainWindow : Window
    {
        JsonFileReader jsonFileReader = new JsonFileReader();

        private ObservableCollection<string> parentLogs = new ObservableCollection<string>();
        private ObservableCollection<string> childLogs = new ObservableCollection<string>();
        private LogGenerator logGenerator = new LogGenerator();
        // Create an instance of InternetOutageDetector
        InternetOutageDetector detector;

        public MainWindow()
        {
            InitializeComponent();

            // Set the data context for the logs list boxes
            ParentLogsListBox.ItemsSource = parentLogs;
            ChildLogsListBox.ItemsSource = childLogs;

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
        }

        // Event handler method for InternetConnectionChanged event
        void HandleInternetConnectionChanged(object sender, InternetConnectionChangedEventArgs e)
        {
            if (e.IsInternetAvailable)
            {
                Console.WriteLine("Internet connection is now available.");
                // Perform actions when internet connection is available
            }
            else
            {
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
        }

        private async void StartLogging()
        {
            while (true)
            {
                // Generate logs using LogGenerator class
                string parentLog = logGenerator.GenerateParentLog();
                string childLog = logGenerator.GenerateChildLog();

                // Add logs to the collections
                AddDataToLog(parentLog, parentLogs, ParentLogsScrollViewer);
                AddDataToLog(childLog, childLogs, ChildLogsScrollViewer);

                // Wait for half a second
                await Task.Delay(500);
            }
        }

        // Example method to add data to the log and scroll to bottom if needed
        private void AddDataToLog(string data, ObservableCollection<string> logCollection, ScrollViewer scrollViewer)
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
    }
}
