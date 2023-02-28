using System.Windows;
using System.Windows.Controls;

namespace Kairos.Actions.Pages
{
    /// <summary>
    /// Interaction logic for AudioVolumePage.xaml
    /// </summary>
    public partial class AudioVolumePage : Page
    {
        AudioVolume audio;
        public AudioVolumePage(AudioVolume output)
        {
            InitializeComponent();
            audio = output;
            volumeSlider.Value = audio.targetVolume;
        }
        private void hideButton_Click(object sender, RoutedEventArgs e)
        {
            audio.Do();
        }
        private void showbutton_Click(object sender, RoutedEventArgs e)
        {
            audio.Undo();
        }
        private void volumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            targetLabel.Content = volumeSlider.Value;
            audio.targetVolume = volumeSlider.Value;
        }
    }
}
