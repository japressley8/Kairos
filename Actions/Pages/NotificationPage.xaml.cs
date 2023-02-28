using System.Windows;
using System.Windows.Controls;

namespace Kairos.Actions.Pages
{
    /// <summary>
    /// Interaction logic for NotificationPage.xaml
    /// </summary>
    public partial class NotificationPage : Page
    {
        Notification notification;
        public NotificationPage(Notification bar)
        {
            InitializeComponent();
            notification = bar;
            invertCheckBox.IsChecked = notification.isInverted;
            msgBox.Text = notification.Msg;
        }
        private void hideButton_Click(object sender, RoutedEventArgs e)
        {
            notification.Do();
        }
        private void showbutton_Click(object sender, RoutedEventArgs e)
        {
            notification.Undo();
        }
        private void invertCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            notification.isInverted = false;
        }
        private void invertCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            notification.isInverted = true;
        }
        private void msgBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            notification.Msg = msgBox.Text;
        }
    }
}
