using System.Windows;
using System.Windows.Controls;

namespace Kairos.Actions.Pages
{
    /// <summary>
    /// Interaction logic for Taskbar.xaml
    /// </summary>
    public partial class TaskbarPage : Page
    {
        Taskbar taskbar;
        public TaskbarPage(Taskbar bar)
        {
            InitializeComponent();
            taskbar = bar;
            invertCheckBox.IsChecked = taskbar.isInverted;
        }
        private void hideButton_Click(object sender, RoutedEventArgs e)
        {
            taskbar.Do();
        }
        private void showbutton_Click(object sender, RoutedEventArgs e)
        {
            taskbar.Undo();
        }
        private void invertCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            taskbar.isInverted = false;
        }
        private void invertCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            taskbar.isInverted = true;
        }
    }
}
