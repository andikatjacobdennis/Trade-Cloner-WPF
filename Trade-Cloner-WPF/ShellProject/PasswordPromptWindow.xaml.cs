using System.Windows;
using System.Windows.Input;

namespace ShellProject
{
    public partial class PasswordPromptWindow : Window
    {
        public string EnteredPassword { get; private set; }

        public PasswordPromptWindow()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            EnteredPassword = PasswordInput.Password;
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            EnteredPassword = null;
            DialogResult = false;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
    }
}
