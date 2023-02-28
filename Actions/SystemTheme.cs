using Microsoft.Win32;
using System.Windows;

namespace Kairos.Actions
{
    public class SystemTheme : Action
    {
        public override string Type { get; set; } = "Change the System Theme";
        public override bool isInverted { get; set; } = false;

        private static readonly string KeyPath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
        RegistryKey key = Registry.CurrentUser.OpenSubKey(KeyPath, true);
        public override void Do() //Change Theme
        {
            if (!isInverted)
                Dark();
            else
                Light();
            //TODO: Refresh?
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
                key.SetValue("SystemUsesLightTheme", 0, RegistryValueKind.DWord);
            }
            catch
            {
                MessageBox.Show("Error changing System Theme: " + this.ToString());
            }
        }
        private void Light()
        {
            try
            {

                key.SetValue("SystemUsesLightTheme", 1, RegistryValueKind.DWord);
            }
            catch
            {
                MessageBox.Show("Error changing System Theme: " + this.ToString());
            }
        }
    }
}
