using System.Windows.Controls;

namespace Kairos.Triggers.Pages
{
    /// <summary>
    /// Interaction logic for StartupPage.xaml
    /// </summary>
    public partial class StartupPage : Page
    {
        Startup startup;
        public StartupPage(Startup start)
        {
            startup = start;
            InitializeComponent();
        }
    }
}
