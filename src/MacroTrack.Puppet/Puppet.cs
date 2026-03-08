// Puppet as in, controling with strings. This will handle all the incoming strings.
using MacroTrack.Puppet.Commands;
using MacroTrack.Core.Services;

namespace MacroTrack.Puppet;

public class Puppet
{
    // Properties
    private readonly Dictionary<string, ICommand> _map;
    private readonly List<ICommand> _commands;

    public Puppet(CoreServices coreServices)
    {
        _commands = Commands.CommandRegistry.CreateAll(coreServices);
        _map = BuildCommandMap(_commands);
    }

    public string Execute(string command)
    {
        var tokens = Tokenize(command);
        if (tokens.Count == 0) return "";

        string commandHead = tokens[0].ToLowerInvariant();
        var args = tokens.Skip(1).ToList();

        if (!_map.TryGetValue(commandHead, out var cmd)) return ($"Unknown command: '{commandHead}', type 'help' for command list.");

        return cmd.Execute(args);
    }

    private static Dictionary<string, ICommand> BuildCommandMap(IEnumerable<ICommand> commands)
    {
        var map = new Dictionary<string, ICommand>(StringComparer.OrdinalIgnoreCase);

        foreach (var cmd in commands)
        {
            map[cmd.Name] = cmd;
            foreach (var alias in cmd.Aliases)
            {
                map[alias] = cmd;
            }
        }
        return map;
    }

    private List<String> Tokenize(string input)
    {
        var tokens = new List<string>();
        if (string.IsNullOrWhiteSpace(input)) return tokens;

        var current = new System.Text.StringBuilder();
        bool inQuotes = false;

        foreach (char c in input.Trim())
        {
            if (c == '"')
            {
                inQuotes = !inQuotes;
                continue;
            }

            if (!inQuotes && char.IsWhiteSpace(c) && current.Length > 0)
            {
                tokens.Add(current.ToString());
                current.Clear();
            }

            else
            {
                current.Append(c);
            }
        }

        if (current.Length > 0)
        {
            tokens.Add(current.ToString());
        }

        return tokens;
    }
}