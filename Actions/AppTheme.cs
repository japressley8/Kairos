using Microsoft.Win32;
using System.Windows;

namespace Kairos.Actions
{
    public class AppTheme : Action
    {
        public override string Type { get; set; } = "Change the App Theme";
        public override bool isInverted { get; set; } = false;

        private static readonly string KeyPath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
        RegistryKey key = Registry.CurrentUser.OpenSubKey(KeyPath, true);
        public override void Do() //Change Theme
        {
            if (!isInverted)
                Dark();
            else
                Light();
        }
        public override void Undo() //Make Light Mode
        {
            if (isInverted)
                Dark();
            else
                Light();
        }
        private void Dark()
        {
            try
            {
                key.SetValue("AppsUseLightTheme", true ? 1 : 0, RegistryValueKind.DWord);
            }
            catch
            {
                MessageBox.Show("Error changing App Theme" + this.ToString());
            }
        }
        private void Light()
        {
            try
            {
                key.SetValue("AppsUseLightTheme", false ? 1 : 0, RegistryValueKind.DWord);
            }
            catch
            {
                MessageBox.Show("Error changing App Theme" + this.ToString());
            }
        }
    }
}
