namespace Kairos.Actions
{
    public class Delay : Action
    {
        public override string Type { get; set; } = "Add a Delay";
        public int numSec { get; set; } = 5;
    }
}
