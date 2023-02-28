using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Kairos.Triggers
{
    public class Fullscreen : Trigger
    {
        public override string Type { get; set; } = "Any Program is Fullscreen";
        public override bool isInverted { get; set; } = false;

        private readonly string[] classWhiteList = new string[]
        {
            //startmeu, taskview (win10), action center etc
            "Windows.UI.Core.CoreWindow",
            //alt+tab screen (win10)
            "MultitaskingViewFrame",
            //taskview (win11)
            "XamlExplorerHostIslandWindow",
            //widget window (win11)
            "WindowsDashboard",
            //taskbar(s)
            "Shell_TrayWnd",
            "Shell_SecondaryTrayWnd",
            //systray notifyicon expanded popup
            "NotifyIconOverflowWindow",
            //rainmeter widgets
            "RainmeterMeterWindow"
        };
        public enum QUERY_USER_NOTIFICATION_STATE
        {
            QUNS_NOT_PRESENT = 1,
            QUNS_BUSY = 2,
            QUNS_RUNNING_D3D_FULL_SCREEN = 3,
            QUNS_PRESENTATION_MODE = 4,
            QUNS_ACCEPTS_NOTIFICATIONS = 5,
            QUNS_QUIET_TIME = 6
        };
        [Flags]
        public enum DwmWindowAttribute : uint
        {
            DWMWA_NCRENDERING_ENABLED = 1,
            DWMWA_NCRENDERING_POLICY,
            DWMWA_TRANSITIONS_FORCEDISABLED,
            DWMWA_ALLOW_NCPAINT,
            DWMWA_CAPTION_BUTTON_BOUNDS,
            DWMWA_NONCLIENT_RTL_LAYOUT,
            DWMWA_FORCE_ICONIC_REPRESENTATION,
            DWMWA_FLIP3D_POLICY,
            DWMWA_EXTENDED_FRAME_BOUNDS,
            DWMWA_HAS_ICONIC_BITMAP,
            DWMWA_DISALLOW_PEEK,
            DWMWA_EXCLUDED_FROM_PEEK,
            DWMWA_CLOAK,
            DWMWA_CLOAKED,
            DWMWA_FREEZE_REPRESENTATION,
            DWMWA_LAST
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public Rectangle ToRectangle()
            {
                return Rectangle.FromLTRB(Left, Top, Right, Bottom);
            }
        }

        private IntPtr workerWOrig, progman;
        public int chosenAlgorithm = -1;
        private const int BOTH_ALGORITHMS = 0;
        private const int FOREGROUND_ALGORITHM = 1;
        private const int GAMEMODE_ALGORITHM = 2;

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        public static extern int GetClassName(int hWnd, StringBuilder lpClassName, int nMaxCount);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr GetShellWindow();
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsZoomed(IntPtr hWnd);
        [DllImport("shell32.dll")]
        public static extern int SHQueryUserNotificationState(out QUERY_USER_NOTIFICATION_STATE pquns);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpWindowClass, string lpWindowName);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, IntPtr windowTitle);
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hwnd, out Rect lpRect);
        [DllImport("dwmapi.dll")]
        static extern int DwmGetWindowAttribute(IntPtr hwnd, int dwAttribute, out Rect pvAttribute, int cbAttribute);
        private void FindDesktopHandles()
        {
            //resetting
            workerWOrig = IntPtr.Zero;
            progman = IntPtr.Zero;

            progman = FindWindow("Progman", null);
            var folderView = FindWindowEx(progman, IntPtr.Zero, "SHELLDLL_DefView", null);
            if (folderView == IntPtr.Zero)
            {
                //If the desktop isn't under Progman, cycle through the WorkerW handles and find the correct one
                do
                {
                    workerWOrig = FindWindowEx(GetDesktopWindow(), workerWOrig, "WorkerW", null);
                    folderView = FindWindowEx(workerWOrig, IntPtr.Zero, "SHELLDLL_DefView", null);
                } while (folderView == IntPtr.Zero && workerWOrig != IntPtr.Zero);
            }
        }
        public override bool Check()
        {
            switch (chosenAlgorithm)
            {
                case BOTH_ALGORITHMS:
                    if (ForegroundAppMonitor())
                    {
                        //MessageBox.Show("ForegroundAppMonitor Triggered");
                        return true;
                    }
                    else if (GameModeAppMonitor())
                    {
                        //MessageBox.Show("GameModeAppMonitor Triggered");
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case FOREGROUND_ALGORITHM:
                    if (ForegroundAppMonitor())
                    {
                        //MessageBox.Show("ForegroundAppMonitor Triggered");
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case GAMEMODE_ALGORITHM:
                    if (GameModeAppMonitor())
                    {
                        //MessageBox.Show("GameModeAppMonitor Triggered");
                        return true;
                    }
                    else
                    {
                        return false;
                    }
            }
            return false;
        }
        private bool IsWhitelistedClass(IntPtr hwnd)
        {
            const int maxChars = 256;
            StringBuilder className = new StringBuilder(maxChars);
            return GetClassName((int)hwnd, className, maxChars) > 0 && classWhiteList.Any(x => x.Equals(className.ToString(), StringComparison.Ordinal));
        }
        public static Rectangle GetWindowRectangle(IntPtr handle)
        {
            Rect rect = new Rect();

            if (Environment.OSVersion.Version.Major >= 6)
            {
                int size = Marshal.SizeOf(typeof(Rect));
                DwmGetWindowAttribute(handle, (int)DwmWindowAttribute.DWMWA_EXTENDED_FRAME_BOUNDS, out rect, size);
            }
            else if (Environment.OSVersion.Version.Major < 6 || rect.ToRectangle().Width == 0)
            {
                GetWindowRect(handle, out rect);
            }

            return rect.ToRectangle();
        }
        private bool ForegroundAppMonitor()
        {
            FindDesktopHandles();
            var fHandle = GetForegroundWindow();

            if (IsWhitelistedClass(fHandle))
            {
                //ignore and resume
                return false;
            }

            try
            {
                GetWindowThreadProcessId(fHandle, out int processID);
                Process fProcess = Process.GetProcessById(processID);

                //process with no name, possibly overlay or some other service pgm
                if (string.IsNullOrEmpty(fProcess.ProcessName) || fHandle.Equals(IntPtr.Zero))
                {
                    //ignore and resume
                    return false;
                }
            }
            catch
            {
                //failed to get process info.. maybe remote process
                //System.Windows.MessageBox.Show("Error, failed to get process info, maybe remote process");
                //ignore and resume
                return false;
            }

            try
            {
                if (!(fHandle.Equals(GetDesktopWindow()) || fHandle.Equals(GetShellWindow())))
                {
                    if (IntPtr.Equals(fHandle, workerWOrig) || IntPtr.Equals(fHandle, progman))
                    {
                        //win10 and win7 desktop foreground while I am running.
                        //ignore and resume
                        return false;
                    }
                    else
                    {
                        Rectangle rect = new Rectangle();
                        Screen[] screens = Screen.AllScreens;
                        foreach (Screen screen in screens)
                        {
                            if (screen != null)
                            {
                                rect = GetWindowRectangle(fHandle);
                                if (screen.Bounds.Height == rect.Height)
                                {
                                    //window covering full screen?
                                    return true;
                                }
                            }
                        }
                    }

                }
            }
            catch { }
            return false;
        }
        private bool GameModeAppMonitor()
        {
            FindDesktopHandles();
            if (SHQueryUserNotificationState(out QUERY_USER_NOTIFICATION_STATE state) == 0)
            {
                switch (state)
                {
                    case QUERY_USER_NOTIFICATION_STATE.QUNS_NOT_PRESENT:
                    case QUERY_USER_NOTIFICATION_STATE.QUNS_BUSY:
                    case QUERY_USER_NOTIFICATION_STATE.QUNS_PRESENTATION_MODE:
                    case QUERY_USER_NOTIFICATION_STATE.QUNS_ACCEPTS_NOTIFICATIONS:
                    case QUERY_USER_NOTIFICATION_STATE.QUNS_QUIET_TIME:
                        break;
                    case QUERY_USER_NOTIFICATION_STATE.QUNS_RUNNING_D3D_FULL_SCREEN:
                        return true;
                }
            }
            return false;
        }
    }
}
