using System.Collections.Generic;

namespace MacroTrack.Core.AppModels;

public static class TimeFrameList
{
    public static readonly List<string> List = new List<string>
    {
        "Calendar day",
        "Last 24 hours",
        "Calendar week",
        "Last 7 days",
        "Calendar month",
        "Last 30 days",
        "Calendar year",
        "Last 365 days"
    };
}