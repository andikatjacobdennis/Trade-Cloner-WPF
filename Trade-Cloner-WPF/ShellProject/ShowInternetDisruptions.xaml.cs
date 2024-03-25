using System.Windows;
using System.Windows.Threading;
using ShellProject.CommonServices.Networking;

namespace ShellProject
{
    /// <summary>
    /// Interaction logic for ShowInternetDisruptions.xaml
    /// </summary>
    public partial class ShowInternetDisruptions : Window
    {
        private InternetOutageDetector internetOutageDetector;
        private DispatcherTimer timer;

        public ShowInternetDisruptions(InternetOutageDetector internetOutageDetector)
        {
            InitializeComponent();
            this.internetOutageDetector = internetOutageDetector;
            OutagesDataGrid.ItemsSource = internetOutageDetector.GetOutagesHistory();

            // Initialize and start the timer
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); // Set the interval to 1 second
            timer.Tick += Timer_Tick; // Assign the event handler for the Tick event
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Refresh the data in the DataGrid every tick
            OutagesDataGrid.ItemsSource = null;
            OutagesDataGrid.ItemsSource = internetOutageDetector.GetOutagesHistory();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
