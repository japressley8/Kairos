using System.Windows;

namespace Kairos.Actions.Pages
{
    /// <summary>
    /// Interaction logic for ManageProgramPage.xaml
    /// </summary>
    public partial class ManageProgramPage : System.Windows.Controls.Page
    {
        ManageProgram program;
        public ManageProgramPage(ManageProgram prog)
        {
            InitializeComponent();
            program = prog;
            invertCheckBox.IsChecked = program.isInverted;
            programLabel.Content = program.Path;
        }
        private void pathButton_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "Program"; // Default file name
            dialog.DefaultExt = ".exe"; // Default file extension
            dialog.Filter = "Program (.exe)|*.exe|Script (.bat)|*.bat"; // Filter files by extension

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                program.Path = dialog.FileName;
                programLabel.Content = dialog.FileName;
            }
        }
        private void hideButton_Click(object sender, RoutedEventArgs e)
        {
            program.Do();
        }
        private void showbutton_Click(object sender, RoutedEventArgs e)
        {
            program.Undo();
        }
        private void invertCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            program.isInverted = false;
        }
        private void invertCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            program.isInverted = true;
        }
    }
}
