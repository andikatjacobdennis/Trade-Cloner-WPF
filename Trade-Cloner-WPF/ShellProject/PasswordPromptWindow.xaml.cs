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

        private void PasswordInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                OKButton_Click(sender, e);
            }
        }

        private void PasswordInput_Loaded(object sender, RoutedEventArgs e)
        {
            PasswordInput.Focus();
        }
    }
}
