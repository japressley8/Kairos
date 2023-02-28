using System;
using System.Windows;
using System.Windows.Controls;

namespace Kairos.Triggers.Pages
{
    /// <summary>
    /// Interaction logic for IdlePage.xaml
    /// </summary>
    public partial class IdlePage : Page
    {
        Idle idle;
        public IdlePage(Idle inIdle)
        {
            idle = inIdle;
            InitializeComponent();
            TimeSpanPickerIdle.Value = idle.idleTime;
        }
        private void TimeSpanPicker_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (TimeSpanPickerIdle.Value != TimeSpan.FromSeconds(0))
            {
                idle.idleTime = TimeSpanPickerIdle.Value;
            }
        }
    }
}
