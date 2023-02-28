using Kairos.Actions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Threading;

namespace Kairos
{
    public class Module
    {
        public int ID { get; set; } = 0;
        public string Name { get; set; }
        public string Description { get; set; }
        public bool isActive { get; set; } = true;
        public List<Container> Containers { get; set; }
        public List<Action> Actions { get; set; }

        private readonly DispatcherTimer dispatcherTimer = new DispatcherTimer();
        private bool wasTriggered = false;
        public Module(string inName = "New Module", string inDescription = "")
        {
            Name = inName;
            Description = inDescription;
            Actions = new List<Action>();
            Containers = new List<Container>();
            dispatcherTimer.Tick += new EventHandler(ProcessMonitor);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 50); //TODO: let the user set the check interval in settings
            Start();
        }
        private void ProcessMonitor(object sender, EventArgs e)
        {
            if (isActive)
            {
                int numTrue = 0;
                foreach (var container in Containers)
                {
                    if (container.Check())
                    {
                        numTrue++;
                    }
                }
                if (numTrue > 0 && !wasTriggered)
                {
                    Thread thread = new Thread(Activate);
                    thread.Start();
                    wasTriggered = true;
                }
                else if (numTrue == 0 && wasTriggered)
                {
                    Thread thread = new Thread(Deactivate);
                    thread.Start();
                    wasTriggered = false;
                }
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
        private void Activate()
        {
            foreach (var action in Actions)
            {
                if (action.Type == "Add a Delay")
                {
                    Thread.Sleep(((Delay)action).numSec * 1000);
                }
                else
                {
                    action.Do();
                }
            }
        }
        private void Deactivate()
        {
            foreach (var action in Actions)
            {
                action.Undo();
            }
        }
        public void StartupCheck()
        {
            foreach (var container in Containers)
            {
                foreach (var trigger in container.Triggers)
                {
                    if (trigger.Type == "On Startup")
                    {
                        trigger.isInverted = true;
                    }
                }
            }
        }
    }
}
