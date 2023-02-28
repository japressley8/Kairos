using JsonSubTypes;
using Kairos.Triggers;
using Newtonsoft.Json;

namespace Kairos
{
    [JsonConverter(typeof(JsonSubtypes), "Type")]
    [JsonSubtypes.KnownSubType(typeof(Hotkey), "Keyboard Shortcut")]
    [JsonSubtypes.KnownSubType(typeof(Schedule), "During a Time Range")]
    [JsonSubtypes.KnownSubType(typeof(Startup), "On Startup")]
    [JsonSubtypes.KnownSubType(typeof(Idle), "On Idle")]
    [JsonSubtypes.KnownSubType(typeof(Fullscreen), "Any Program is Fullscreen")]
    [JsonSubtypes.KnownSubType(typeof(ProgramDetect), "Specific Program is Running")]
    [JsonSubtypes.KnownSubType(typeof(Charging), "Computer is Charging")]
    public class Trigger
    {
        public virtual string Type { get; set; }
        public virtual bool isInverted { get; set; }
        public virtual bool Check() { return false; }
    }
}
