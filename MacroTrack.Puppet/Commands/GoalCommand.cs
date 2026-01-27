using MacroTrack.Core.Services;
using MacroTrack.Core.Models;

namespace MacroTrack.Puppet.Commands;

public class GoalCommand : ICommand
{
    private readonly GoalService _service;

    public string Name => "goal";
    public IReadOnlyList<string> Aliases => new[] { "g", "goals" };
    public string Description => "Commands for interfacing with Goals.";
    public string Usage => "goal [activate/add/deleteactivation/delete/edit/getcurrent/history/list] (other arguments)";
    public string LongHelp => @"Command set for interfacing with GoalRegistry and GoalHistory. Subcommands are:
     - goal activate [ID] (date)            - Activates given preset: Uses ID to take goal from GoalRegistry, generates GoalActivation with it, adds this to the end of GoalHistory.
     - goal add [name] [calories] [protein] [carbs] [fat] (notes) (type) (minCal) (maxCal) (minPro) (maxPro) (minCarb) (maxCarb) (minFat) (maxFat)
                                            - Generates new goal and adds it to GoalRegistry, leave entries as blank (space between double quotations) for null. Min and Max parameters will usually be left as null.
     - goal deleteactivation [ID]            - Deletes the selected goal activation from GoalHistory. Use 'goal history' to see history and IDs.
     - goal delete [ID]                     - Deletes the selected goal from GoalRegistry. Use 'goal list' to see Registry and IDs. Will not delete a goal which has been used before. If you really want to delete all trace of such a goal, you will need to delete every use of it from GoalHistory with goal deactivate.
     - goal edit [ID] [name] [calories] [protein] [carbs] [fat] (notes) (type) (minCal) (maxCal) (minPro) (maxPro) (minCarb) (maxCarb) (minFat) (maxFat)
                                            - Sets the goal in GoalRegistry with ID to the specified amounts. If you want to keep data for one entry, use '_' (we can't use 'null', beacuse the optional data can be set and stored as null). Use 'goal list' to see GoalRegistry and IDs.
     - goal getcurrent (date)               - Returns the current goal as of today's date. If a date is specified, returns the activated goal for that date.
     - goal history (startDate) (endDate)   - Returns GoalHistory. Returns all with no arguments, all from startDate to today if first argument specified, all between startDate and endDate if both specified.
     - goal list                            - Returns GoalRegistry";

    
    public GoalCommand(GoalService service)
    {
        _service = service;
    }

    public string Execute(IReadOnlyList<string> args)
    {
        if (args.Count == 0) return $"Not Enough arguments, usage: {Usage}";
        switch (args[0])
        {
            case "activate": return Activate(args.Skip(1).ToList());
            case "add": return Add(args.Skip(1).ToList());
            case "deleteactivation": return DeleteActivation(args.Skip(1).ToList());
            case "delete": return Delete(args.Skip(1).ToList());
            case "edit": return Edit(args.Skip(1).ToList());
            case "getcurrent": return GetCurrent(args.Skip(1).ToList());
            case "history": return History(args.Skip(1).ToList());
            case "list": return ListRegistry(args.Skip(1).ToList());
            case "pizza": return Pizza(args.Skip(1).ToList());
            default: return $"Unknown argument 'goal {args[0]}', type 'help goal'.";
        }
    }

    private string Pizza(IReadOnlyList<string> args)
    {
        try 
        {
            _service.EnsureDeactivationEntries();
            return $"Pizza: EnsureDeactivationEntries() has output, so here is a slice of pizza: (>)";
        }
        catch (Exception ex) {return $"Puppet.Commands.GoalCommand.Pizza(): Error ensuring deactivation entries: {ex.Message}";}
    }

    private string Activate(IReadOnlyList<string> args)
    {
        if (args.Count == 0) return $"Not enough arguments, usage: goal activate [ID] (date)";
        if (!int.TryParse(args[0], out int id)) return $"Invalid ID '{args[0]}', must be an integer.";
        if (args.Count == 1) 
        {
            try {return $"Activated:\n{PrintActivation(_service.ActivateGoal(id))}";}
            catch (Exception ex) {return $"Puppet.Commands.GoalCommand.Activate(): (1 argument) Error activating goal, probably wrong ID, try 'goal list' to see goal registry and IDs: {ex.Message}";}

        }
        if (!DateOnly.TryParse(args[1], out DateOnly date)) return $"Invalid Date '{args[1]}', try format 'YYYY/MM/DD'";
        try {return $"Activated:\n{PrintActivation(_service.ActivateGoal(id, date))}";}
        catch (Exception ex) {return $"Puppet.Commands.GoalCommand.Activate(): (2 arguments) Error activating goal, probably wrong ID, try 'goal list' to see goal registry and IDs: {ex.Message}";}
    }

    private string Add(IReadOnlyList<string> args)
    {
        if (args.Count < 5) return "Not enough arguments, usage: goal add [name] [calories] [protein] [carbs] [fat] (notes) (type) (minCal) (maxCal) (minPro) (maxPro) (minCarb) (maxCarb) (minFat) (maxFat)";

        // Make sure the first 5 arguments work.
        string goalName = args[0];
        if (!double.TryParse(args[1], out double calories)) return $"Invalid calorie amount '{args[1]}', must be a number";
        if (!double.TryParse(args[2], out double protein)) return $"Invalid protein amount '{args[2]}', must be a number";
        if (!double.TryParse(args[3], out double carbs)) return $"Invalid carbs amount '{args[3]}', must be a number";
        if (!double.TryParse(args[4], out double fat)) return $"Invalid fat amount '{args[4]}', must be a number";

        // Minimum size:
        if (args.Count == 5) 
        {
            try {return PrintEntry(_service.AddGoal(goalName, calories, protein, carbs, fat));}
            catch (Exception ex) {return $"Puppet.Commands.GoalCommand.Add(): 5 args, error adding goal: {ex.Message}";}
        }

        // there might be a quicker way to do this, but I don't know it:
        string? notes = null;
        string? type = null;
        if (args.Count > 5 && !string.IsNullOrWhiteSpace(args[5])) notes = args[5];
        if (args.Count > 6 && !string.IsNullOrWhiteSpace(args[6])) type = args[6];

        double? MinCal = null;
        double? MaxCal = null;
        double? MinPro = null;
        double? MaxPro = null;
        double? MinCar = null;
        double? MaxCar = null;
        double? MinFat = null;
        double? MaxFat = null;
        
        if (args.Count > 7 && double.TryParse(args[7], out double minCal)) MinCal = minCal;
        if (args.Count > 8 && double.TryParse(args[8], out double maxCal)) MaxCal = maxCal;
        if (args.Count > 9 && double.TryParse(args[9], out double minPro)) MinPro = minPro;
        if (args.Count > 10 && double.TryParse(args[10], out double maxPro)) MaxPro = maxPro;
        if (args.Count > 11 && double.TryParse(args[11], out double minCar)) MinCar = minCar;
        if (args.Count > 12 && double.TryParse(args[12], out double maxCar)) MaxCar = maxCar;
        if (args.Count > 13 && double.TryParse(args[13], out double minFat)) MinFat = minFat;
        if (args.Count > 14 && double.TryParse(args[14], out double maxFat)) MaxFat = maxFat; 

        try {return PrintEntry(_service.AddGoal(goalName, calories, protein, carbs, fat, notes, type, MinCal, MaxCal, MinPro, MaxPro, MinCar, MaxCar, MinFat, MaxFat));}
        catch (Exception ex) {return $"Puppet.Commands.GoalCommand.Add(): 15 args, error adding goal: {ex.Message}";}
    }

    public string DeleteActivation(IReadOnlyList<string> args)
    {
        if (args.Count == 0) return "Not enough arguments, usage: goal deactivate [ID], use 'goal history' to see history and IDs.";
        if (!int.TryParse(args[0], out int id)) return $"Invalid ID '{args[0]}', must be an integer.";
        try { return $"Removing goal activation:\n{PrintActivation(_service.DeleteGoalActivation(id))}";}
        catch (Exception ex) {return $"Puppet.Commands.GoalCommand.Deactivate(): Error deactivating. Wrong ID probably, use 'goal history' to see history and IDs: {ex.Message}";}
    }

    public string Delete(IReadOnlyList<string> args)
    {
        if (args.Count == 0) return "Not enough arguments, usage: goal delete [ID]";
        if (!int.TryParse(args[0], out int id)) return $"Invalid ID '{args[0]}', must be an integer.";
        try {return $"Deleted goal: {PrintEntry(_service.DeleteGoal(id))}";}
        catch (Exception ex) {return $"Puppet.Commands.GoalCommand.Delete(): Error deleting goal: {ex.Message}";}
    }

    public string Edit(IReadOnlyList<string> args)
    {
        if (args.Count < 6) return "Not enough arguments, usage: goal edit [ID] [name] [calories] [protein] [carbs] [fat] (notes) (type) (minCal) (maxCal) (minPro) (maxPro) (minCarb) (maxCarb) (minFat) (maxFat)";

        double? parseDouble(string input, double? presentValue)
        {
            if (string.IsNullOrWhiteSpace(input)) return null;
            if (double.TryParse(input, out double output)) return output;
            if (input == "_") return presentValue;
            return null; // we could throw an exception here but who cares.
        }

        // Make sure the first 6 arguments work.
        if (!int.TryParse(args[0], out int id)) return $"Invalid ID '{args[0]}', must be an integer.";
        Goal goal;
        try { goal = _service.GetGoal(id); }
        catch (Exception ex) { return $"Puppet.Commands.GoalCommand.Edit(): Goal doesn't seem to exist, wrong ID? Use 'goal list' for goal registry and IDs: {ex.Message}"; }
        string goalName = args[1] == "_" ? goal.GoalName : args[1];

        double calories = parseDouble(args[2], goal.Calories) ?? 
            throw new Exception($"Invalid calorie amount '{args[2]}', must be a number or '_' for previous value");
        double protein = parseDouble(args[3], goal.Protein) ?? 
            throw new Exception($"Invalid protein amount '{args[3]}', must be a number or '_' for previous value");
        double carbs = parseDouble(args[4], goal.Carbs) ?? 
            throw new Exception($"Invalid carbs amount '{args[4]}', must be a number or '_' for previous value");
        double fat = parseDouble(args[5], goal.Fat) ?? 
            throw new Exception($"Invalid fat amount '{args[5]}', must be a number or '_' for previous value");

        // Minimum size:
        if (args.Count == 6) 
        {
            try {return PrintEntry(_service.EditGoal(id, goalName, calories, protein, carbs, fat));}
            catch (Exception ex) {return $"Puppet.Commands.GoalCommand.Edit(): 6 args, error editing goal: {ex.Message}";}
        }

        // there might be a quicker way to do this, but I don't know it:
        string? notes = null;
        string? type = null;
        if (args.Count > 6 && !string.IsNullOrWhiteSpace(args[6])) notes = args[6] == "_" ? goal.Notes : args[6];
        if (args.Count > 7 && !string.IsNullOrWhiteSpace(args[7])) type = args[7] == "_" ? goal.GoalType : args[7];

        double? MinCal = goal.MinCal;
        double? MaxCal = goal.MaxCal;
        double? MinPro = goal.MinPro;
        double? MaxPro = goal.MaxPro;
        double? MinCar = goal.MinCar;
        double? MaxCar = goal.MaxCar;
        double? MinFat = goal.MinFat;
        double? MaxFat = goal.MaxFat;

        if (args.Count > 8) MinCal = parseDouble(args[8], MinCal);
        if (args.Count > 9) MaxCal = parseDouble(args[9], MaxCal);
        if (args.Count > 10) MinPro = parseDouble(args[10], MinPro);
        if (args.Count > 11) MaxPro = parseDouble(args[11], MaxPro);
        if (args.Count > 12) MinCar = parseDouble(args[12], MinCar);
        if (args.Count > 13) MaxCar = parseDouble(args[13], MaxCar);
        if (args.Count > 14) MinFat = parseDouble(args[14], MinFat);
        if (args.Count > 15) MaxFat = parseDouble(args[15], MaxFat); 

        try {return PrintEntry(_service.EditGoal(id, goalName, calories, protein, carbs, fat, notes, type, MinCal, MaxCal, MinPro, MaxPro, MinCar, MaxCar, MinFat, MaxFat));}
        catch (Exception ex) {return $"Puppet.Commands.GoalCommand.Edit(): error editing goal: {ex.Message}";}
    }

    public string GetCurrent(IReadOnlyList<string> args)
    {
        if (args.Count == 0)
        {
            try
            {
                var ga = _service.GetPresentGoal();
                if (ga == null) return "No active goal found as of now.";
                var cg = _service.G(ga);
                return $"Active goal as of now ({DateOnly.FromDateTime(DateTime.Now).ToString("yyyy/MM/dd")}) is:\n{PrintEntry(cg)}\nActivated on {ga.ActivatedAt.ToString("yyyy/MM/dd")}";
            }
            catch (Exception ex) {return $"Puppet.Commands.GoalCommand.GetCurrent(): Error getting current goal: {ex.Message}";}
        }
        if (!DateOnly.TryParse(args[0], out DateOnly date)) return $"Invalid Date '{args[0]}', try format 'YYYY/MM/DD'";
        try
        {
            var ga = _service.GetPresentGoal(date);
            if (ga == null) return $"No active goal found as of {date.ToString("yyyy/MM/dd")}.";
            var cg = _service.G(ga);
            return $"Active goal as of date {date.ToString("yyyy/MM/dd")} is:\n{PrintEntry(cg)}\nActivated on {ga.ActivatedAt.ToString("yyyy/MM/dd")}";
        }
        catch (Exception ex) {return $"Puppet.Commands.GoalCommand.GetCurrent(): Error getting current goal at time: {ex.Message}";}
    }

    public string History(IReadOnlyList<string> args)
    {
        return PrintActivationList(_service.GetGoalHistory());
    }

    public string ListRegistry(IReadOnlyList<string> args)
    {
        return PrintGoalList(_service.GetAllGoals());
    }

    public string PrintEntry(Goal goal)
    {
        // Since we're being cute about formatting, let's make it long and nice.
        const int lengthID = 7;
        const int lengthName = 30;
        const int lengthType = 10;
        const int lengthCalories = 7;
        const int lengthMacro = 6;
        const int lengthNotes = 100;

        string Id = Truncate($"[{goal.Id}]", lengthID);
        string GoalName = Truncate(goal.GoalName, lengthName);
        string GoalType = goal.GoalType == null ? Truncate("-", lengthType) : Truncate(goal.GoalType, lengthType);

        string Calories = Truncate(goal.Calories.ToString(), lengthCalories);
        string Protein = Truncate(goal.Protein.ToString(), lengthMacro);
        string Carbs = Truncate(goal.Carbs.ToString(), lengthMacro);
        string Fat = Truncate(goal.Fat.ToString(), lengthMacro);

        string MinCal = goal.MinCal == null? Truncate("-", lengthCalories) : Truncate(goal.MinCal.ToString()!, lengthCalories);
        string MaxCal = goal.MaxCal == null? Truncate("-", lengthCalories) : Truncate(goal.MaxCal.ToString()!, lengthCalories);
        string MinPro = goal.MinPro == null? Truncate("-", lengthMacro) : Truncate(goal.MinPro.ToString()!, lengthMacro);
        string MaxPro = goal.MaxPro == null? Truncate("-", lengthMacro) : Truncate(goal.MaxPro.ToString()!, lengthMacro);
        string MinCar = goal.MinCar == null? Truncate("-", lengthMacro) : Truncate(goal.MinCar.ToString()!, lengthMacro);
        string MaxCar = goal.MaxCar == null? Truncate("-", lengthMacro) : Truncate(goal.MaxCar.ToString()!, lengthMacro);
        string MinFat = goal.MinFat == null? Truncate("-", lengthMacro) : Truncate(goal.MinFat.ToString()!, lengthMacro);
        string MaxFat = goal.MaxFat == null? Truncate("-", lengthMacro) : Truncate(goal.MaxFat.ToString()!, lengthMacro);
        string Notes = goal.Notes == null? Truncate("-", lengthNotes) : Truncate(goal.Notes, lengthNotes);

        string entryString = $"{Id, -lengthID}{GoalName, -lengthName}{GoalType, -lengthType}    {Calories, -lengthCalories} | {Protein, -lengthMacro} | {Carbs, -lengthMacro} | {Fat, -lengthMacro} | {MinCal, lengthCalories}:{MaxCal, -lengthCalories} | {MinPro, lengthMacro}:{MaxPro, -lengthMacro} | {MinCar, lengthMacro}:{MaxCar, -lengthMacro} | {MinFat, lengthMacro}:{MaxFat, -lengthMacro} | {Notes, -lengthNotes}";
        return entryString;
    }

    public string PrintGoalList(List<Goal> goalList)
    {
        const int lengthID = 7;
        const int lengthName = 30;
        const int lengthType = 10;
        const int lengthCalories = 7;
        const int lengthMacro = 6;

        // header:
        string stringList = $"{"ID:", -lengthID}{"Goal Name:", -lengthName}{"Goal type:", -lengthType}    {"Cal:", -lengthCalories} | {"Pro…:", -lengthMacro} | {"Carbs:", -lengthMacro} | {"Fat:", -lengthMacro} | {"Min/Max Cal:", lengthCalories * 2}  | {"Min/Max Pro:", lengthMacro * 2}  | {"Min/Max Car:", lengthMacro * 2}  | {"Min/Max Fat:", lengthMacro * 2}\n";
        foreach (Goal g in goalList)
        {
            stringList += PrintEntry(g) + "\n";
        }
        return stringList;
    }

    public string PrintActivation(GoalActivation ga)
    {
        return $"{$"[{ga.Id}]",-7}{$"[{ga.GoalId}]",-7}{ga.ActivatedAt:yyyy/MM/dd}{(ga.DeactivatedAt == DateOnly.MinValue ? "" : $" - {ga.DeactivatedAt:yyyy/MM/dd}")}";
    }

    public string PrintActivationList(List<GoalActivation> gaList)
    {
        string stringList = $"{"ID:",-7}{"GoalID:",-7}\n";
        foreach (GoalActivation ga in gaList)
        {
            stringList += PrintActivation(ga) + "\n";
        }
        return stringList;
    }

    public string Truncate(string input, int length)
    {
        if (input.Length > length) return input[..(length -1)] + "…";
        else return input;
    }
}