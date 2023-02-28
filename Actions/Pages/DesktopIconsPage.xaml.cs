using System.Windows;
using System.Windows.Controls;

namespace Kairos.Actions.Pages
{
    /// <summary>
    /// Interaction logic for DesktopIconsPage.xaml
    /// </summary>
    public partial class DesktopIconsPage : Page
    {
        DesktopIcons icons;
        public DesktopIconsPage(DesktopIcons action)
        {
            InitializeComponent();
            icons = action;
            invertCheckBox.IsChecked = icons.isInverted;
        }
        private void hideButton_Click(object sender, RoutedEventArgs e)
        {
            icons.Do();
        }
        private void showbutton_Click(object sender, RoutedEventArgs e)
        {
            icons.Undo();
        }
        private void invertCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            icons.isInverted = false;
        }
        private void invertCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            icons.isInverted = true;
        }
    }
}
