using System;
using System.Configuration;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Kairos
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        Configuration config;
        MainWindow mainWindow;
        public SettingsPage(MainWindow main)
        {
            InitializeComponent();
            config = ConfigurationManager.OpenExeConfiguration(System.Windows.Forms.Application.ExecutablePath);
            pathLabel.Content = config.AppSettings.Settings["ModuleDir"].Value;
            mainWindow = main;
        }
        private void pathButton_Click(object sender, RoutedEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                string[] files = Directory.GetFiles(ConfigurationManager.OpenExeConfiguration(System.Windows.Forms.Application.ExecutablePath).AppSettings.Settings["ModuleDir"].Value, "*.kairos", SearchOption.AllDirectories);
                CommonOpenFileDialog dialog = new CommonOpenFileDialog();
                dialog.IsFolderPicker = true;
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    var entry = config.AppSettings.Settings["ModuleDir"];
                    if (entry == null)
                        config.AppSettings.Settings.Add("ModuleDir", dialog.FileName);
                    else
                        config.AppSettings.Settings["ModuleDir"].Value = dialog.FileName;

                    config.Save(ConfigurationSaveMode.Modified);
                }
                pathLabel.Content = config.AppSettings.Settings["ModuleDir"].Value;
            }
            mainWindow.Refresh();
        }
        private void importButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "Module"; // Default file name
            dialog.DefaultExt = ".kairos"; // Default file extension
            dialog.Filter = "Module (.kairos)|*.kairos"; // Filter files by extension

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                FileMan.Save(FileMan.Load(dialog.FileName));
                mainWindow.Refresh();
                mainWindow.navFrame.Navigate(null);
            }
        }
        private void exportButton_Click(object sender, RoutedEventArgs e)
        {
            exportList.ItemsSource = mainWindow.modules;
            exportCard.Visibility = Visibility.Visible;
        }
        private void exportList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(exportList.SelectedIndex != -1)
            {
                string[] files = Directory.GetFiles(ConfigurationManager.OpenExeConfiguration(System.Windows.Forms.Application.ExecutablePath).AppSettings.Settings["ModuleDir"].Value, "*.kairos", SearchOption.AllDirectories);
                string dest = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                CommonOpenFileDialog dialog = new CommonOpenFileDialog();
                dialog.IsFolderPicker = true;
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    dest = dialog.FileName;

                    foreach (var file in files)
                    {
                        Module module = FileMan.Load(file);
                        if (module.ID == ((Module)exportList.SelectedItem).ID)
                        {
                            File.Copy(file, dest + "\\module" + module.ID + ".kairos", true);
                            break;
                        }
                    }
                }
                exportCard.Visibility = Visibility.Hidden;
                exportList.SelectedIndex = -1;
            }
        }
    }
}
