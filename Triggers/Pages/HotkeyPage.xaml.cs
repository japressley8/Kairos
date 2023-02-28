using System.Windows.Controls;

namespace Kairos.Triggers.Pages
{
    /// <summary>
    /// Interaction logic for HotkeyPage.xaml
    /// </summary>
    public partial class HotkeyPage : Page
    {
        Hotkey hotkey;
        public HotkeyPage(Hotkey key)
        {
            hotkey = key;
            InitializeComponent();
            Display();
        }
        private int GetMod(int num)
        {
            switch(num)
            {
                case 0: return 1;
                case 1: return 2;
                case 2: return 4;
                case 3: return 8;
            }
            return -1;
        }
        private int GetKey(string ch)
        {
            switch(ch.ToUpper())
            {
                case "A":
                    return 65;
                case "B":
                    return 66;
                case "C":
                    return 67;
                case "D":
                    return 68;
                case "E":
                    return 69;
                case "F":
                    return 70;
                case "G":
                    return 71;
                case "H":
                    return 72;
                case "I":
                    return 73;
                case "J":
                    return 74;
                case "K":
                    return 75;
                case "L":
                    return 76;
                case "M":
                    return 77;
                case "N":
                    return 78;
                case "O":
                    return 79;
                case "P":
                    return 80;
                case "Q":
                    return 81;
                case "R":
                    return 82;
                case "S":
                    return 83;
                case "T":
                    return 84;
                case "U":
                    return 85;
                case "V":
                    return 86;
                case "W":
                    return 87;
                case "X":
                    return 88;
                case "Y":
                    return 89;
                case "Z":
                    return 90;
            }
            return -1;
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(keyBox != null && keyBox.Text.Length == 1 && modBox1.SelectedIndex != -1)
            {
                if(modBox2.SelectedIndex < 4 && modBox2.SelectedIndex != modBox1.SelectedIndex)
                {
                    hotkey.Modifiers = (ModifierKeyCodes)(GetMod(modBox2.SelectedIndex) | GetMod(modBox1.SelectedIndex));
                }
                else
                {
                    hotkey.Modifiers = (ModifierKeyCodes)GetMod(modBox1.SelectedIndex);
                }
                hotkey.Key = (VirtualKeyCodes)GetKey(keyBox.Text);
                hotkey.Register();
            }
        }
        private void keyBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ComboBox_SelectionChanged(sender, null);
        }
        private void Display()
        {
            switch(hotkey.Modifiers)
            {
                case ModifierKeyCodes.Alt:
                    modBox1.SelectedIndex = 0;
                    break;
                case ModifierKeyCodes.Control:
                    modBox1.SelectedIndex = 1;
                    break;
                case ModifierKeyCodes.Shift:
                    modBox1.SelectedIndex = 2;
                    break;
                case ModifierKeyCodes.Windows:
                    modBox1.SelectedIndex = 3;
                    break;
                case ModifierKeyCodes.Alt | ModifierKeyCodes.Control:
                    modBox1.SelectedIndex = 0;
                    modBox2.SelectedIndex = 1;
                    break;
                case ModifierKeyCodes.Alt | ModifierKeyCodes.Shift:
                    modBox1.SelectedIndex = 0;
                    modBox2.SelectedIndex = 2;
                    break;
                case ModifierKeyCodes.Alt | ModifierKeyCodes.Windows:
                    modBox1.SelectedIndex = 0;
                    modBox2.SelectedIndex = 3;
                    break;
                case ModifierKeyCodes.Control | ModifierKeyCodes.Shift:
                    modBox1.SelectedIndex = 1;
                    modBox2.SelectedIndex = 2;
                    break;
                case ModifierKeyCodes.Control | ModifierKeyCodes.Windows:
                    modBox1.SelectedIndex = 1;
                    modBox2.SelectedIndex = 3;
                    break;
                case ModifierKeyCodes.Shift | ModifierKeyCodes.Windows:
                    modBox1.SelectedIndex = 2;
                    modBox2.SelectedIndex = 3;
                    break;
            }
            if (hotkey.Key.ToString() != "0")
            {
                keyBox.Text = hotkey.Key.ToString();
            }
        }
    }
}
