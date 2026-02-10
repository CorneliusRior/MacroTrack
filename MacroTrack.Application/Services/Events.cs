using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.AppLibrary.Services
{
    /*
    Publish like AppServices.AppEvents.Publish(new Event());
    Subscribe like _subEvent = AppServices.AppEvents.Subscribe<Event>(_ => Method());
        and make sure to put _subEvent.Dispose() on window close!
        
    If you want to pass arguments with these events, you can do it like this:
        public sealed record SettingsChanged(type argument, type argument);
        AppServices.AppEvents.Publish(new Event(argument, argument);
        _subEvent = AppServices.AppEvents.Subscribe<Event>(msg =>
            {
                meg.Arg1 ...
                msg.Arg2 ...
            });

    For types which have the method EventSubscribe(IDisposable e); and _subscription list you can do this:
        EventSubscribe(AppServices.AppEvents.Subscribe<Event>(msg =>
            {
                msg.Arg1 ...
            });
    or just (_ => Method()) if you prefer.
    */
    public sealed record SettingsChanged;
    public sealed record SummaryChanged;
    public sealed record FoodLogChanged;
    public sealed record PresetListChanged;
    public sealed record TaskCompletionChanged;
}
