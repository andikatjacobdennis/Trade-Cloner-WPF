using System;
using System.Threading;
using System.Windows;

namespace ShellProject
{
    public static class SingleInstanceManager
    {
        private const string MutexName = "ShellProjectMutex";
        private static Mutex mutex;

        public static bool Initialize()
        {
            bool createdNew;
            mutex = new Mutex(true, MutexName, out createdNew);
            return createdNew;
        }

        public static void Cleanup()
        {
            if (mutex != null)
            {
                mutex.ReleaseMutex();
                mutex.Close();
                mutex = null;
            }
        }
    }

    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            bool createdNew = SingleInstanceManager.Initialize();

            if (!createdNew)
            {
                // Another instance is already running, so exit
                MessageBox.Show("Another instance of the program is already running.", "ShellProject", MessageBoxButton.OK, MessageBoxImage.Information);
                SingleInstanceManager.Cleanup();
                Environment.Exit(0);
            }

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            SingleInstanceManager.Cleanup();
            base.OnExit(e);
        }
    }
}
