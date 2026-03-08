using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.Core.Models
{
    public enum GoalType 
    { 
        None, 
        Custom, 
        Cut, 
        Maintenance, 
        Bulk
    }
    public static class GoalTypeExtensions
    {
        public static string GetString(this GoalType type)
        {
            return type switch
            {
                GoalType.None => "None",
                GoalType.Custom => "Custom",
                GoalType.Cut => "Cut",
                GoalType.Maintenance => "Maintenance",
                GoalType.Bulk => "Bulk",
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public static string GetDescription(this GoalType type)
        {
            return type switch
            {
                GoalType.None => "None selected",
                GoalType.Custom => "Custom label specified for this goal type.",
                GoalType.Cut => "Caloric deficit diet, where caloric intake is roughly identical to maintenance caloric intake, used to lose mass (fat).",
                GoalType.Maintenance => "Caloric maintenance diet, where caloric intake is roughly identical to maintenance caloric intake, used for mass maintenance and recomposition.",
                GoalType.Bulk => "Caloric surplus diet, where caloric intake exceeds maintenance caloric intake, used to gain mass (muscle).",
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public static GoalType ToGoalType(this string input)
        {
            return input.ToLowerInvariant().Trim() switch
            {
                // actual strings:
                "none" => GoalType.None,
                "custom" => GoalType.Custom,
                "cut" => GoalType.Cut,
                "maintenance" => GoalType.Maintenance,
                "bulk" => GoalType.Bulk,
                
                // other strings:
                "default" => GoalType.None,
                "userdefined" => GoalType.Custom,
                "deficit" => GoalType.Cut,
                "maintain" => GoalType.Maintenance,
                "surplus" => GoalType.Bulk,

                // numbers:
                "0" => GoalType.None,
                "1" => GoalType.Custom,
                "2" => GoalType.Cut,
                "3" => GoalType.Maintenance,
                "4" => GoalType.Bulk,

                _ => throw new ArgumentException()
            };
        }
    }

    public sealed record GoalTypeListItem(GoalType Value, string DisplayName);

    public static class GoalTypeList
    {
        public static readonly IReadOnlyList<GoalTypeListItem> NoCustom =
        [
            new(GoalType.None, "(None)"),
            new(GoalType.Cut, "Cut"),
            new(GoalType.Maintenance, "Maintenance"),
            new(GoalType.Bulk, "Bulk")
        ];

        public static readonly IReadOnlyList<GoalTypeListItem> WithCustom =
        [
            new(GoalType.None, "(None)"),
            new(GoalType.Custom, "Custom"),
            new(GoalType.Cut, "Cut"),
            new(GoalType.Maintenance, "Maintenance"),
            new(GoalType.Bulk, "Bulk")
        ];
    }
}
