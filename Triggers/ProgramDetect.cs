using System.Diagnostics;
using System.IO;

namespace Kairos.Triggers
{
    public class ProgramDetect : Trigger
    {
        public override string Type { get; set; } = "Specific Program is Running"; 
        public override bool isInverted { get; set; } = false;
        public string programPath = null;

        public override bool Check()
        {
            if (programPath != null)
            {
                string FilePath = Path.GetDirectoryName(programPath);
                string FileName = Path.GetFileNameWithoutExtension(programPath).ToLower();

                Process[] pList = Process.GetProcessesByName(FileName);

                if (pList.Length > 0)
                {
                    return !isInverted;
                }
            }
            return isInverted;
        }
    }
}
