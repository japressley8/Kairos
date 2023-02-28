using System;
using System.Windows;
using System.Windows.Controls;

namespace Kairos.Triggers.Pages
{
    /// <summary>
    /// Interaction logic for SchedulePage.xaml
    /// </summary>
    public partial class SchedulePage : Page
    {
        Schedule schedule;
        bool init = true;
        public SchedulePage(Schedule span)
        {
            schedule = span;
            InitializeComponent();
            invertCheckBox.IsChecked = schedule.isInverted;
            fromTime.SelectedTime = schedule.start;
            toTime.SelectedTime = schedule.end;
            init = false;
        }
        private void invertCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            schedule.isInverted = false;
        }
        private void invertCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            schedule.isInverted = true;
        }
        private void Time_SelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            if (!init)
            {
                schedule.start = fromTime.SelectedTime;
                schedule.end = toTime.SelectedTime;
            }
        }
    }
}
