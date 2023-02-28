using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System;
using System.ComponentModel;
using System.IO;
using System.Configuration;
using System.Reflection;

namespace Kairos
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NotifyIcon m_notifyIcon;
        System.Windows.Controls.ContextMenu menu = new System.Windows.Controls.ContextMenu();
        private WindowState m_storedWindowState = WindowState.Normal;
        public List<Module> modules { get; set; } = new List<Module>();
        public MainWindow()
        {
            InitializeComponent();

            //Run me on startup
            try
            {
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                Assembly curAssembly = Assembly.GetExecutingAssembly();
                key.SetValue(curAssembly.GetName().Name, curAssembly.Location);
            }
            catch { }

            //Get, Check, and Set Module Directory
            var config = ConfigurationManager.OpenExeConfiguration(System.Windows.Forms.Application.ExecutablePath);

            var entry = config.AppSettings.Settings["ModuleDir"];

            if (entry == null)
            {
                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Kairos"));
                config.AppSettings.Settings.Remove("ModuleDir");
                config.AppSettings.Settings.Add("ModuleDir", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Kairos"));
                config.Save(ConfigurationSaveMode.Modified);
            }

            //Tray Icon
            m_notifyIcon = new NotifyIcon();
            m_notifyIcon.BalloonTipText = "Kairos has been minimised. Click the tray icon to show.";
            m_notifyIcon.BalloonTipTitle = "Kairos";
            m_notifyIcon.Text = "Kairos";
            m_notifyIcon.Icon = Properties.Resources.Kairos_Icon; //TODO, crashes when in release mode
            m_notifyIcon.MouseDown += new MouseEventHandler(m_notifyIcon_Click);

            System.Windows.Controls.MenuItem item = new System.Windows.Controls.MenuItem();
            item.Header = "Exit";
            item.Click += exitTrayIconMenu_Click;

            menu.Items.Add(item);

            //Load Modules
            Refresh();

            //start application minimized to tray
            Hide();
            m_notifyIcon.Visible = true;

            foreach(Module module in modules)
            {
                module.StartupCheck();
            }
        }
        public void Refresh()
        {
            modules = FileMan.Load();
            moduleList.ItemsSource = modules;
        }
        protected override void OnClosing(CancelEventArgs e) //override the close button, and minimize it to the tray
        {
            e.Cancel = true;

            WindowState = WindowState.Minimized;
            OnStateChanged(EventArgs.Empty);
            CheckTrayIcon();
        }
        private void OnStateChanged(object sender, EventArgs args) //hide window or resore it
        {
            if (WindowState == WindowState.Minimized)
            {
                Hide();
            }
            else
            {
                m_storedWindowState = WindowState;
            }
        }
        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            CheckTrayIcon(); 
        }
        private void m_notifyIcon_Click(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                menu.IsOpen = true;
            }
            else
            {
                Show();
                WindowState = m_storedWindowState;
                menu.IsOpen = false;
            }
        }
        private void CheckTrayIcon() //toggle tray icon visibility
        {
            ShowTrayIcon(!IsVisible);
        }
        private void ShowTrayIcon(bool show)
        {
            if (m_notifyIcon != null)
            {
                m_notifyIcon.Visible = show;
            }
        }
        private void exitTrayIconMenu_Click(object sender, RoutedEventArgs e) //close the app
        {
            System.Windows.Application.Current.Shutdown();
        }
        private void newModuleButton_Click(object sender, RoutedEventArgs e)
        {
            Module module = new Module();
            modules.Add(module);
            moduleList.Items.Refresh();
            moduleList.SelectedIndex = modules.Count - 1;
            FileMan.Save(module);
        }
        private void moduleList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (moduleList.SelectedIndex != -1)
            {
                navFrame.Navigate(new ModuleTabs((Module)moduleList.SelectedItem));
                moduleList.Items.Refresh();
            }
        }
        private void moduleList_RemoveItem(object sender, RoutedEventArgs e)
        {
            FileMan.Delete((Module)moduleList.SelectedItem);
            modules.Remove((Module)moduleList.SelectedItem);
            moduleList.Items.Refresh();
            navFrame.Navigate(null);
        }
        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            moduleList.SelectedIndex = -1;
            navFrame.Navigate(new SettingsPage(this));
        }
        private void activationToggle_Checked(object sender, RoutedEventArgs e)
        {
            FileMan.Update((Module)moduleList.SelectedItem);
        }
    }
}
