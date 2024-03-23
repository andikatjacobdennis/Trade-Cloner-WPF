using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ShellProject
{
    /// <summary>
    /// Interaction logic for ShowInternetDisruptions.xaml
    /// </summary>
    public partial class ShowInternetDisruptions : Window
    {
        private InternetOutageDetector internetOutageDetector;
        public ShowInternetDisruptions(InternetOutageDetector internetOutageDetector)
        {
            InitializeComponent();
            this.internetOutageDetector = internetOutageDetector;
            OutagesDataGrid.ItemsSource = internetOutageDetector.GetOutagesHistory();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            OutagesDataGrid.ItemsSource = null;
            OutagesDataGrid.ItemsSource = internetOutageDetector.GetOutagesHistory();
        }
    }
}
