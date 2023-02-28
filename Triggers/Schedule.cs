using System;

namespace Kairos.Triggers
{
    public class Schedule : Trigger
    {
        public override string Type { get; set; } = "During a Time Range";
        public override bool isInverted { get; set; } = false; 
        public DateTime? start = null;
        public DateTime? end = null;
        public override bool Check()
        {
            if (start != null && end != null && DateTime.Now > start && DateTime.Now < end)
            {
                return !isInverted;
            }
            else
            {
                return isInverted;
            }
        }
    }
}
