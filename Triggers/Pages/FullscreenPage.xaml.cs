using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Kairos.Triggers.Pages
{
    /// <summary>
    /// Interaction logic for FullscreenPage.xaml
    /// </summary>
    public partial class FullscreenPage : Page
    {
        Fullscreen fullscreen;
        public FullscreenPage(Fullscreen inFullscreen)
        {
            InitializeComponent();
            fullscreen = inFullscreen;
            invertCheckBox.IsChecked = fullscreen.isInverted;
            Display();
        }
        private void invertCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            fullscreen.isInverted = true;
        }
        private void invertCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            fullscreen.isInverted = false;
        }
        private void exclusiveButton_Click(object sender, RoutedEventArgs e)
        {
            fullscreen.chosenAlgorithm = 2;
            Display();
        }
        private void borderlessButton_Click(object sender, RoutedEventArgs e)
        {
            fullscreen.chosenAlgorithm = 1;
            Display();
        }
        private void bothButton_Click(object sender, RoutedEventArgs e)
        {
            fullscreen.chosenAlgorithm = 0;
            Display();
        }
        private void Display()
        {
            if (fullscreen.chosenAlgorithm == 0)
            {
                bothButton.Background = Brushes.SlateGray;
                exclusiveButton.ClearValue(Button.BackgroundProperty);
                borderlessButton.ClearValue(Button.BackgroundProperty);
            }
            else if (fullscreen.chosenAlgorithm == 1)
            {
                bothButton.ClearValue(Button.BackgroundProperty);
                exclusiveButton.ClearValue(Button.BackgroundProperty);
                borderlessButton.Background = Brushes.SlateGray;
            }
            else if (fullscreen.chosenAlgorithm == 2)
            {
                bothButton.ClearValue(Button.BackgroundProperty);
                exclusiveButton.Background = Brushes.SlateGray;
                borderlessButton.ClearValue(Button.BackgroundProperty);
            }
        }
    }
}
