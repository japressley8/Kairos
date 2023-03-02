using System.Collections.Generic;
using System.Windows;
using AudioSwitcher.AudioApi.CoreAudio;

namespace Kairos.Actions
{
    public class AudioOutput : Action
    {
        public override string Type { get; set; } = "Change the Audio Output";

        private IEnumerable<CoreAudioDevice> devices = new CoreAudioController().GetPlaybackDevices();
        private static CoreAudioDevice defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
        private CoreAudioDevice prevDefaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
        public string targetDevice { get; set; } = defaultPlaybackDevice.FullName;
        public IEnumerable<CoreAudioDevice> getDevices()
        {
            return devices;
        }
        private void ChangeOutput(string op)
        {
            if (op != defaultPlaybackDevice.FullName)
            {
                prevDefaultPlaybackDevice = defaultPlaybackDevice;
                foreach (CoreAudioDevice d in devices)
                {
                    if (d.FullName == op)
                    {
                        d.SetAsDefault();
                        defaultPlaybackDevice = d;
                    }
                }
            }
        }
        private void RevertOutput()
        {
            prevDefaultPlaybackDevice.SetAsDefault();
            defaultPlaybackDevice = prevDefaultPlaybackDevice;
        }
        public override void Do()
        {
            if (targetDevice != null)
            {
                ChangeOutput(targetDevice);
            }
            else
            {
                MessageBox.Show("Target Device is null in Do(): " + this.ToString());
            }
        }
        public override void Undo()
        {
            if (targetDevice != null)
            {
                RevertOutput();
            }
            else
            {
                MessageBox.Show("Target Device is null in Undo(): " + this.ToString());
            }
        }
    }
}
