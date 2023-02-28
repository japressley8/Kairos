using System.Windows;
using System.Windows.Controls;

namespace Kairos.Actions.Pages
{
    /// <summary>
    /// Interaction logic for DelayPage.xaml
    /// </summary>
    public partial class DelayPage : Page
    {
        Delay delay = null;
        public DelayPage(Delay inDelay)
        {
            InitializeComponent();
            delay = inDelay;
            timeSlider.Value = delay.numSec;
            timeLabel.Content = timeSlider.Value.ToString();
        }
        private void timeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (delay != null)
            {
                timeLabel.Content = timeSlider.Value.ToString();
                delay.numSec = (int)timeSlider.Value;
            }
        }
    }
}
