using MacroTrack.Core.Services;
using MacroTrack.Puppet2.Commands;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;

namespace MacroTrack.Puppet2
{
    public sealed class PuppetEngine : IPuppetContext
    {
        private readonly Dictionary<string, IPuppetCommand> _map;
        public PuppetEngine(CoreServices services)
        {
            var commands = DiscoverCommands(services);
            _map = BuildCommandMap(commands);
        }

        public PuppetResult Execute(string input)
        {
            var tokens = Tokenize(input);
            if (tokens.Count == 0) return PuppetResult.Ok("");
            var head = tokens[0].ToLowerInvariant();
            var args = tokens.Skip(1).ToList();
            if (!_map.TryGetValue(head, out var cmd)) return PuppetResult.Fail($"Unknown command '{head}', type 'help'.");
            try { return cmd.Execute(args); }
            catch (PuppetUserException ex) { return PuppetResult.Fail(ex.Message); }
            catch (Exception ex) { return PuppetResult.Fail($"Command '{head}' failed: {ex.Message}"); }
        }
        
        // Discover commands:
        private IEnumerable<IPuppetCommand> DiscoverCommands(CoreServices services)
        {
            var commandType = typeof(IPuppetCommand);
            var types = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => commandType.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
            foreach (var type in types) yield return (IPuppetCommand)Activator.CreateInstance(type, services, this)!;
        }

        // Command Map:
        private static Dictionary<string, IPuppetCommand> BuildCommandMap(IEnumerable<IPuppetCommand> commands)
        {
            var map = new Dictionary<string, IPuppetCommand>(StringComparer.OrdinalIgnoreCase);
            foreach (IPuppetCommand cmd in commands)
            {
                map[cmd.Name] = cmd;
                foreach (string alias in cmd.Aliases) map[alias] = cmd;
            }
            return map;
        }

        // CommandList:
        public IReadOnlyCollection<IPuppetCommand> CommandList => _map.Values.Distinct().ToList();

        /// <summary>
        /// TryGetCommand: will get the command even if it is an alias, for use outside (help &c.)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool TryGetCommand(string name, [NotNullWhen(true)] out IPuppetCommand? command) =>_map.TryGetValue(name, out command);        

        // Tokenizer (upgrade to read machine/human?)
        private static List<string> Tokenize(string input)
        {
            List<string> tokens = new();
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

    public interface IPuppetContext
    {
        IReadOnlyCollection<IPuppetCommand> CommandList { get; }
        bool TryGetCommand(string name, [NotNullWhen(true)] out IPuppetCommand? command);
    }
}
