using JsonSubTypes;
using Kairos.Actions;
using Newtonsoft.Json;

namespace Kairos
{
    [JsonConverter(typeof(JsonSubtypes), "Type")]
    [JsonSubtypes.KnownSubType(typeof(Delay), "Add a Delay")]
    [JsonSubtypes.KnownSubType(typeof(ManageProgram), "Launch/Close a Program")]
    [JsonSubtypes.KnownSubType(typeof(Wallpaper), "Change the Wallpaper")]
    [JsonSubtypes.KnownSubType(typeof(Taskbar), "Hide/Show the Taskbar")]
    [JsonSubtypes.KnownSubType(typeof(DesktopIcons), "Hide/Show the Desktop Icons")]
    [JsonSubtypes.KnownSubType(typeof(SystemTheme), "Change the System Theme")]
    [JsonSubtypes.KnownSubType(typeof(AppTheme), "Change the App Theme")]
    [JsonSubtypes.KnownSubType(typeof(AudioOutput), "Change the Audio Output")]
    [JsonSubtypes.KnownSubType(typeof(AudioVolume), "Change the Volume")]
    [JsonSubtypes.KnownSubType(typeof(Notification), "Display a Notification")]
    public class Action
    {
        public virtual string Type { get; set; }
        public virtual bool isInverted { get; set; }
        public virtual void Do() { }
        public virtual void Undo() { }
    }
}
