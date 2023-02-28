using AudioSwitcher.AudioApi.CoreAudio;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Kairos.Actions.Pages
{
    /// <summary>
    /// Interaction logic for AudioOutputPage.xaml
    /// </summary>
    public partial class AudioOutputPage : Page
    {
        AudioOutput audio;
        public AudioOutputPage(AudioOutput output)
        {
            InitializeComponent();
            audio = output;
            ChangeAudioComboBox.ItemsSource = audio.getDevices();
            ChangeAudioComboBox.DisplayMemberPath = "FullName";
            for (int i = 0; i < audio.getDevices().Count(); i++)
            {
                if (audio.getDevices().ElementAt(i).FullName == audio.targetDevice)
                {
                    ChangeAudioComboBox.SelectedIndex = i;
                }
            }
        }
        private void ChangeAudioComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            audio.targetDevice = ((CoreAudioDevice)(ChangeAudioComboBox.SelectedItem)).FullName;
        }
        private void hideButton_Click(object sender, RoutedEventArgs e)
        {
            audio.Do();
        }
        private void showbutton_Click(object sender, RoutedEventArgs e)
        {
            audio.Undo();
        }
    }
}
