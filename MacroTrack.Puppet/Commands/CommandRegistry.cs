using MacroTrack.Core.Services;

namespace MacroTrack.Puppet.Commands;

public static class CommandRegistry
{
    public static List<ICommand> CreateAll(CoreServices coreServices)
    {
        var commands = new List<ICommand>();

        commands.Add(new DiaryCommand(coreServices.diaryService));
        commands.Add(new FoodCommand(coreServices.foodLogService, coreServices.presetService));
        commands.Add(new GoalCommand(coreServices.goalService));
        commands.Add(new PresetCommand(coreServices.presetService));
        commands.Add(new PrintCommand());
        commands.Add(new TaskCommand(coreServices.taskService));
        commands.Add(new WeightCommand(coreServices.weightLogService));

        commands.Add(new HelpCommand(() => commands));
        return commands;
    }
}