using System.Runtime.InteropServices;

namespace Kairos.Actions
{
    public class Taskbar : Action
    {
        public override string Type { get; set; } = "Hide/Show the Taskbar";
        public override bool isInverted { get; set; } = false;

        [DllImport("user32.dll")]
        private static extern int FindWindow(string className, string windowText);
        [DllImport("user32.dll")]
        public static extern int ShowWindow(int hwnd, int command);
        public static int primary = FindWindow("Shell_TrayWnd", null);
        public static int secondary = FindWindow("Shell_SecondaryTrayWnd", null);
        //TODO: Make work on > 2 monitors
        //https://github.com/TimUntersberger/nog/issues/85
        public override void Do() //Hide taskbars
        {
            if (!isInverted)
                Hide();
            else
                Show();
        }
        public override void Undo() //Show taskbars
        {
            if (!isInverted)
                Show();
            else
                Hide();
        }
        private void Hide()
        {
            ShowWindow(primary, 0);
            ShowWindow(secondary, 0);
        }
        private void Show()
        {
            ShowWindow(primary, 1);
            ShowWindow(secondary, 1);
        }
    }
}
