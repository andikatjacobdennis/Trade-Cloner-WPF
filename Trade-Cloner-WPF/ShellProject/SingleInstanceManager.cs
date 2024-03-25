namespace ShellProject
{
    /// <summary>
    /// Manages single instance behavior of the application using a Mutex.
    /// </summary>
    public static class SingleInstanceManager
    {
        // Unique name for the Mutex to ensure single instance behavior
        private const string MutexName = "xt5BfMr6BCSCeeRLNsjT9QdJSG5lEl5y";
        private static Mutex? mutex;

        /// <summary>
        /// Initializes the SingleInstanceManager by creating a Mutex.
        /// </summary>
        /// <returns>True if a new instance is created; otherwise, false.</returns>
        public static bool Initialize()
        {
            mutex = new Mutex(true, MutexName, out bool createdNew);
            return createdNew;
        }

        /// <summary>
        /// Cleans up resources by releasing and closing the Mutex.
        /// </summary>
        public static void Cleanup()
        {
            if (mutex != null)
            {
                mutex.ReleaseMutex(); // Release the Mutex to allow other instances
                mutex.Close(); // Close the Mutex
                mutex = null; // Set the Mutex object to null for cleanup
            }
        }
    }
}
