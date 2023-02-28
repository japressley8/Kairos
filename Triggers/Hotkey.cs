using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Kairos.Triggers
{
    [Flags]
    public enum ModifierKeyCodes : uint
    {
        Alt = 1,
        Control = 2,
        Shift = 4,
        Windows = 8
    }

    /// <summary>
    /// Virtual Key Codes
    /// </summary>
    public enum VirtualKeyCodes : uint
    {
        A = 65,
        B = 66,
        C = 67,
        D = 68,
        E = 69,
        F = 70,
        G = 71,
        H = 72,
        I = 73,
        J = 74,
        K = 75,
        L = 76,
        M = 77,
        N = 78,
        O = 79,
        P = 80,
        Q = 81,
        R = 82,
        S = 83,
        T = 84,
        U = 85,
        V = 86,
        W = 87,
        X = 88,
        Y = 89,
        Z = 90
    }
    public class Hotkey : Trigger
    {
        public override string Type { get; set; } = "Keyboard Shortcut"; 

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, ModifierKeyCodes fdModifiers, VirtualKeyCodes vk);

        #region Fields
        WindowInteropHelper host;
        private bool IsDisposed = false;
        private int Identifier;
        public bool isActive = false;
        private Window Window { get; set; }
        public VirtualKeyCodes Key { get; set; }
        public ModifierKeyCodes Modifiers { get; set; }
        #endregion
        public Hotkey()
        {
            Window = new Window();
            host = new WindowInteropHelper(Window);
            Identifier = Window.GetHashCode();
        }
        public void Register()
        {
            RegisterHotKey(host.Handle, Identifier, Modifiers, Key);
            ComponentDispatcher.ThreadPreprocessMessage += ProcessMessage;
        }
        void ProcessMessage(ref MSG msg, ref bool handled)
        {
            if ((msg.message == 786) && (msg.wParam.ToInt32() == Identifier)) //TODO: idk why it is 786, it just works
                Triggered();
        }
        public void Triggered()
        {
            isActive = !isActive;
        }
        public void Dispose()
        {
            if (!IsDisposed)
            {
                ComponentDispatcher.ThreadPreprocessMessage -= ProcessMessage;

                UnregisterHotKey(host.Handle, Identifier);
                Window = null;
                host = null;
            }
            IsDisposed = true;
        }
        public override bool Check()
        {
            return isActive;
        }
    }
}
