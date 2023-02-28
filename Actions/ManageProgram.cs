using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace Kairos.Actions
{
    public class ManageProgram : Action
    {
        public override string Type { get; set; } = "Launch/Close a Program";
        public override bool isInverted { get; set; } = false;
        public string Path { get; set; } = "";

        [Flags]
        private enum ProcessAccessFlags : uint
        {
            QueryLimitedInformation = 0x00001000
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool QueryFullProcessImageName(
              [In] IntPtr hProcess,
              [In] int dwFlags,
              [Out] StringBuilder lpExeName,
              ref int lpdwSize);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr OpenProcess(
         ProcessAccessFlags processAccess,
         bool bInheritHandle,
         int processId);

        static String GetProcessFilename(Process p)
        {
            int capacity = 2000;
            StringBuilder builder = new StringBuilder(capacity);
            IntPtr ptr = OpenProcess(ProcessAccessFlags.QueryLimitedInformation, false, p.Id);
            if (!QueryFullProcessImageName(ptr, 0, builder, ref capacity))
            {
                return String.Empty;
            }

            return builder.ToString();
        }
        private static void Launch(string programPath)
        {
            Process objProcess = new Process();
            objProcess.StartInfo.FileName = programPath;
            objProcess.Start();
        }
        private static void Close(string programPath)
        {
            Process[] runningProcesses = Process.GetProcesses();
            foreach (Process process in runningProcesses)
            {
                // now check the modules of the process
                if (GetProcessFilename(process) == programPath)
                {
                    process.Kill();
                }
            }
        }
        public override void Do()
        {
            if (Path != "")
            {
                if(!isInverted)
                {
                    Launch(Path);
                }
                else
                {
                    Close(Path);
                }
            }
            else
            {
                MessageBox.Show("Make sure to select a program first");
            }
        }
        public override void Undo()
        {
            if (Path != "")
            {
                if (!isInverted)
                {
                    Close(Path);
                }
                else
                {
                    Launch(Path);
                }
            }
            else
            {
                MessageBox.Show("Make sure to select a program first");
            }
        }
    }
}
