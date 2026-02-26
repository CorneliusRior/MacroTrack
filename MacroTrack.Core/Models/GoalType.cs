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
    }
}
