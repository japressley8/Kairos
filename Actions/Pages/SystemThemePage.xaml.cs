using System.Windows;
using System.Windows.Controls;

namespace Kairos.Actions.Pages
{
    /// <summary>
    /// Interaction logic for SystemThemePage.xaml
    /// </summary>
    public partial class SystemThemePage : Page
    {
        SystemTheme theme;
        public SystemThemePage(SystemTheme action)
        {
            InitializeComponent();
            theme = action;
            invertCheckBox.IsChecked = theme.isInverted;
        }
        private void hideButton_Click(object sender, RoutedEventArgs e)
        {
            theme.Do();
        }
        private void showbutton_Click(object sender, RoutedEventArgs e)
        {
            theme.Undo();
        }
        private void invertCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            theme.isInverted = false;
        }
        private void invertCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            theme.isInverted = true;
        }
    }
}
