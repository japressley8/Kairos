using AudioSwitcher.AudioApi.CoreAudio;
using System.Windows;

namespace Kairos.Actions
{
    public class AudioVolume : Action
    {
        public override string Type { get; set; } = "Change the Volume";
        
        private bool wasActivated = false;

        private static CoreAudioController controller = new CoreAudioController();
        private CoreAudioDevice defaultPlaybackDevice = controller.DefaultPlaybackDevice;
        public double prevVolume = 0;
        public double targetVolume = 0;
        private void ChangeVolume(double volume)
        {
            prevVolume = defaultPlaybackDevice.Volume;
            if (volume > 0)
            {
                defaultPlaybackDevice.Mute(false);
            }
            defaultPlaybackDevice.Volume = volume;
        }
        private void RevertVolume()
        {
            defaultPlaybackDevice.Volume = prevVolume;
        }
        public override void Do()
        {
            if (!wasActivated)
            {
                if (targetVolume >= 0)
                {
                    defaultPlaybackDevice = controller.DefaultPlaybackDevice;
                    ChangeVolume(targetVolume);
                    wasActivated = true;
                }
                else
                {
                    MessageBox.Show("Target Volume is negative in Do(): " + this.ToString());
                }
            }
        }
        public override void Undo()
        {
            if (wasActivated)
            {
                if (targetVolume >= 0)
                {
                    defaultPlaybackDevice = controller.DefaultPlaybackDevice;
                    RevertVolume();
                    wasActivated = false;
                }
                else
                {
                    MessageBox.Show("Target Volume is negative in Undo(): " + this.ToString());
                }
            }
        }
    }
}
