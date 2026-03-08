namespace MacroTrack.Puppet.Commands;

public interface ICommand
{
    string Name { get; }
    IReadOnlyList<string> Aliases { get; }
    string Description { get; }
    string Usage { get; }
    string LongHelp { get; }

    string Execute(IReadOnlyList<string> args);
}