using MacroTrack.Core.Services;
using MacroTrack.Puppet2.Commands;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
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
            foreach (string s in tokens) p(s);
            if (tokens.Count == 0) return PuppetResult.Ok("");

            var head = TokenizeCommandHead(tokens[0]);
            foreach (string s in head) p(s);
            var args = tokens.Skip(1).ToList();
            foreach (string s in args) p(s);

            // if the first argument is "help" or "?", we pass that to HelpCommand.

            if (args.Count > 0) if (args[0].ToLowerInvariant().Trim() == "help" || args[0].ToLowerInvariant().Trim() == "?")
            {
                try { return _map.GetValueOrDefault("help")!.Execute(TokenizeCommandHead("help"), Tokenize(tokens[0])); }
                catch (PuppetUserException ex) { return PuppetResult.Fail(ex.Message); }
                catch (Exception ex) { return PuppetResult.Fail($"Command '{head}' failed: {ex.Message}"); }
            }

            if (!_map.TryGetValue(head[0], out var cmd)) return PuppetResult.Fail($"Unknown command '{head[0]}', type 'help'.");
            try { return cmd.Execute(head, args); }
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

        // Tokenizer
        private static List<string> Tokenize(string input)
        {
            List<string> tokens = new();
            if (string.IsNullOrWhiteSpace(input)) return tokens;

            var current = new StringBuilder();
            bool inQuotes = false;
            int braceDepth = 0;

            foreach (char c in input.Trim())
            {
                if (c == '"')
                {
                    inQuotes = !inQuotes;
                    if (braceDepth > 0) current.Append(c);
                    continue;
                }
                if (!inQuotes)
                {
                    if (c == '{') braceDepth++;
                    if (c == '}') braceDepth--;
                }    
                if (!inQuotes && char.IsWhiteSpace(c) && current.Length > 0 && braceDepth == 0)
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

        // Subcommand tokenizer, used to split "Diary.Add"/"Diary.Delete" &c. if we want to do it that way.
        private static List<string> TokenizeCommandHead(string head)
        {
            List<string> tokens = new();
            if (string.IsNullOrWhiteSpace(head)) return tokens;

            var current = new StringBuilder();
            foreach (char c in head.Trim())
            {
                if (c == '.')
                {
                    tokens.Add(current.ToString());
                    current.Clear();
                }
                else current.Append(c);
            }
            if (current.Length > 0) tokens.Add(current.ToString());
            return tokens;
        }

        /// <summary>
        /// Better debugging printer. Ignore all parameters except "Message", the rest fill in automatically.
        /// </summary>
        /// <param name="message"></param>
        public static void p(string message, [CallerMemberName] string member = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
        {
            Debug.WriteLine($"{Path.GetFileName(file)} line {line} {member}(): {message}");
        }
    }
    
    public interface IPuppetContext
    {
        IReadOnlyCollection<IPuppetCommand> CommandList { get; }
        bool TryGetCommand(string name, [NotNullWhen(true)] out IPuppetCommand? command);
    }
}
