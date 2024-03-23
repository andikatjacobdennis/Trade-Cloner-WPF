using System;
using System.Collections.Generic;
using System.Timers;
using Timer = System.Timers.Timer;

namespace ShellProject
{
    public class InternetOutageDetector : IDisposable
    {
        private bool isInternetAvailable;
        private DateTime outageStartTime;
        private DateTime? outageEndTime;
        private int outageDurationSeconds;
        private int totalOutages;
        private Timer timer;
        private List<Tuple<DateTime, DateTime?, int>> outageHistory;

        public event EventHandler<InternetConnectionChangedEventArgs> InternetConnectionChanged;

        public InternetOutageDetector()
        {
            outageHistory = new List<Tuple<DateTime, DateTime?, int>>();

            // Check initial network status
            isInternetAvailable = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
            if (!isInternetAvailable)
            {
                totalOutages++;
                outageStartTime = DateTime.Now;
                outageHistory.Add(new Tuple<DateTime, DateTime?, int>(outageStartTime, null, 0)); // Add row for outage start
                isInternetAvailable = false;
                OnInternetConnectionChanged(new InternetConnectionChangedEventArgs(false));
            }
            else
            {
                isInternetAvailable = true;
            }
        }

        private void CheckInternetStatus(object sender, ElapsedEventArgs e)
        {
            bool isCurrentlyAvailable = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (isInternetAvailable && !isCurrentlyAvailable) // Internet outage started
            {
                totalOutages++;
                outageStartTime = DateTime.Now;
                outageHistory.Add(new Tuple<DateTime, DateTime?, int>(outageStartTime, null, 0)); // Add row for outage start
                isInternetAvailable = false;
                OnInternetConnectionChanged(new InternetConnectionChangedEventArgs(false));
            }
            else if (!isInternetAvailable && isCurrentlyAvailable) // Internet outage ended
            {
                outageEndTime = DateTime.Now;
                outageDurationSeconds = (int)(outageEndTime.Value - outageStartTime).TotalSeconds;

                // Update the last added row in outage history with end time and duration
                if (outageHistory.Count > 0) // Check if outageHistory is not empty
                {
                    outageHistory[outageHistory.Count - 1] = new Tuple<DateTime, DateTime?, int>(
                        outageStartTime,
                        outageEndTime,
                        outageDurationSeconds
                    );
                }

                OnInternetConnectionChanged(new InternetConnectionChangedEventArgs(true));
                isInternetAvailable = true;
            }

            // Update duration for ongoing outages before returning history
            UpdateOngoingOutageDurations();
        }

        protected virtual void OnInternetConnectionChanged(InternetConnectionChangedEventArgs e)
        {
            InternetConnectionChanged?.Invoke(this, e);
        }

        public List<Tuple<DateTime, DateTime?, int>> GetOutagesHistory()
        {
            // Update duration for ongoing outages before returning history
            UpdateOngoingOutageDurations();
            return outageHistory;
        }

        public int GetTotalOutages()
        {
            // Update duration for ongoing outages before returning history
            UpdateOngoingOutageDurations();
            return totalOutages;
        }

        private void UpdateOngoingOutageDurations()
        {
            DateTime currentTime = DateTime.Now;
            var tempOutageHistory = new List<Tuple<DateTime, DateTime?, int>>(outageHistory); // Create a copy

            foreach (var outage in tempOutageHistory)
            {
                if (!outage.Item2.HasValue) // Outage is ongoing
                {
                    int durationSeconds = (int)(currentTime - outage.Item1).TotalSeconds;
                    outageHistory[outageHistory.IndexOf(outage)] =
                        new Tuple<DateTime, DateTime?, int>(outage.Item1, null, durationSeconds);
                }
            }
        }

        public void Start()
        {
            timer = new Timer(1000); // Check internet status every second
            timer.Elapsed += CheckInternetStatus;
            timer.Enabled = true;

            // Check internet status at startup and handle accordingly
            if (!isInternetAvailable)
            {
                totalOutages++;
                outageStartTime = DateTime.Now;
                isInternetAvailable = false;
                OnInternetConnectionChanged(new InternetConnectionChangedEventArgs(false));
            }
        }

        public void Stop()
        {
            timer.Stop();
            timer.Dispose();
        }

        public void Dispose()
        {
            timer.Dispose();
        }

        internal object GetLastOutage()
        {
            return outageStartTime;
        }
    }

    public class InternetConnectionChangedEventArgs : EventArgs
    {
        public bool IsInternetAvailable { get; }

        public InternetConnectionChangedEventArgs(bool isInternetAvailable)
        {
            IsInternetAvailable = isInternetAvailable;
        }
    }
}
