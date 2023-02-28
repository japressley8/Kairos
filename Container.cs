using System.Collections.Generic;

namespace Kairos
{
    public class Container
    {
        public List<Trigger> Triggers { get; set; }
        public Container(Trigger trigger)
        {
            Triggers = new List<Trigger>();
            Triggers.Add(trigger);
        }
        public bool Closed()
        {
            foreach(Trigger trigger in Triggers)
            {
                if(trigger.Type == "On Startup")
                {
                    return true;
                }
            }
            return false;
        }
        public bool Check()
        {
            foreach(Trigger trigger in Triggers)
            {
                if(!trigger.Check())
                {
                    return false;
                }
            }
            return true;
        }
    }
}
