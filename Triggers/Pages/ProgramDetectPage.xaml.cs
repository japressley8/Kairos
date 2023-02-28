using System.Windows;
using System.Windows.Controls;

namespace Kairos.Triggers.Pages
{
    /// <summary>
    /// Interaction logic for ProgramDetectPage.xaml
    /// </summary>
    public partial class ProgramDetectPage : Page
    {
        ProgramDetect program;
        public ProgramDetectPage(ProgramDetect app)
        {
            InitializeComponent();
            program = app;
            invertCheckBox.IsChecked = program.isInverted;
            programLabel.Content = program.programPath;
        }
        private void pathButton_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "Program"; // Default file name
            dialog.DefaultExt = ".exe"; // Default file extension
            dialog.Filter = "Program (.exe)|*.exe"; // Filter files by extension

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                program.programPath = dialog.FileName;
                programLabel.Content = program.programPath;
            }
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
