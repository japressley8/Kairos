namespace Kairos.Triggers
{
    public class Charging : Trigger
    {
        public override string Type { get; set; } = "Computer is Charging";
        public override bool isInverted { get; set; } = false;
        public override bool Check()
        {
            if (System.Windows.Forms.SystemInformation.PowerStatus.PowerLineStatus == System.Windows.Forms.PowerLineStatus.Offline)
            {
                return isInverted;
            }
            return !isInverted;
        }
    }
}
