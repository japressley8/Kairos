using System.Windows;
using System.Windows.Controls;

namespace Kairos.Triggers.Pages
{
    /// <summary>
    /// Interaction logic for ChargingPage.xaml
    /// </summary>
    public partial class ChargingPage : Page
    {
        Charging charging;
        public ChargingPage(Charging charge)
        {
            charging = charge;
            InitializeComponent();
            invertCheckBox.IsChecked = charging.isInverted;
        }
        private void invertCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            charging.isInverted = true;
        }
        private void invertCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            charging.isInverted = false;
        }
    }
}
