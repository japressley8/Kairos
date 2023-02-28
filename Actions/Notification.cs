using Microsoft.Toolkit.Uwp.Notifications;
using System.Windows;

namespace Kairos.Actions
{
    public class Notification : Action //Microsoft.Toolkit.Uwp.Notifications
    {
        public override string Type { get; set; } = "Display a Notification";
        public override bool isInverted { get; set; } = false;
        public string Msg { get; set; } = "";
        public override void Do()
        {
            if (Msg != "")
            {
                if (!isInverted)
                {
                    new ToastContentBuilder()
                        .AddText(Msg)
                        .Show();
                }
            }
            else
            {
                MessageBox.Show("Make sure to set the notification message");
            }
        }
        public override void Undo()
        {
            if (Msg != "")
            {
                if (isInverted)
                {
                    new ToastContentBuilder()
                        .AddText(Msg)
                        .Show();
                }
            }
            else
            {
                MessageBox.Show("Make sure to set the notification message");
            }
        }
    }
}