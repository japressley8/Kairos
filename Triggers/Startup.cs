namespace Kairos.Triggers
{
    public class Startup : Trigger
    {
        public override string Type { get; set; } = "On Startup";
        public override bool isInverted { get; set; } = false;
        public override bool Check()
        {
            return isInverted;
        }
    }
}
