using System;
using System.Windows.Threading;

namespace Kairos
{
    internal class Watcher
    {
        public bool isActive = false;
        private readonly DispatcherTimer dispatcherTimer = new DispatcherTimer();
        private Func<bool> check;
        private Func<bool, bool> action;
        public Watcher(Func<bool> Check, Func<bool, bool> Action)
        {
            check = Check;
            action = Action;
            dispatcherTimer.Tick += new EventHandler(ProcessMonitor);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 5);
        }
        private void ProcessMonitor(object sender, EventArgs e)
        {
            if (!isActive && check())
            {
                action(true);
                isActive = true;
            }
            else if (isActive && !check())
            {
                action(false);
                isActive = false;
            }
        }
        public void Start()
        {
            dispatcherTimer.Start();
        }

        public void Stop()
        {
            if (dispatcherTimer.IsEnabled)
            {
                dispatcherTimer.Stop();
            }
        }
    }
}
