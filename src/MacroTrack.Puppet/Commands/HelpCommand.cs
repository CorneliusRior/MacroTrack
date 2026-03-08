namespace MacroTrack.Puppet.Commands;

public class HelpCommand: ICommand{
    private readonly Func<IEnumerable<ICommand>> _getCommands;

    public HelpCommand(Func<IEnumerable<ICommand>> getCommands)
    {
        _getCommands = getCommands;
    }
    
    public string Name => "help";
    public IReadOnlyList<string> Aliases => new[] { "h", "?" };
    public string Description => "Lists all commands, or gives details for one command.";
    public string Usage => "help (command)";
    public string LongHelp => "Standard help command. Type 'help' in the console on its own and it will list every listed command in this program. If you want more details on an individual command, type 'help <command>' and it will print a written,longer description like this one.\nGiven the construction of the program, as of writing I'm not actually sure if this message will be visible.";

    public string Execute(IReadOnlyList<string> args)
    {
        var commands = _getCommands()
            .GroupBy(c => c.Name, StringComparer.OrdinalIgnoreCase)
            .Select(g => g.First())
            .OrderBy(c => c.Name)
            .ToList();

        if (args.Count == 0)
        {
            var lines = commands.Select(c => $"{c.Name,-12} {c.Description}\n");
            string HelpList = "All commands, type 'help <command>' for more details:\n";
            foreach (var line in lines) 
            {
                HelpList += line;
            }
            return HelpList;
        }

        var target = args[0];

        var match = commands.FirstOrDefault(c => c.Name.Equals(target, StringComparison.OrdinalIgnoreCase) || c.Aliases.Any(a => a.Equals(target, StringComparison.OrdinalIgnoreCase)));

        if (match == null)
        {
            return $"Unknown command: '{target}', type 'help' for full command list.";
        }

        return $"{match.Name}\n{match.Description}\nUsage: {match.Usage}\nAliases: {string.Join(", ", match.Aliases)}\n{match.LongHelp}\n";
        
    }
}