using MacroTrack.Core.Models;
using MacroTrack.Core.Services;
using MacroTrack.Puppet2.Commands;
using MacroTrack.Puppet2.Scripting;
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
        private readonly IProgress<ScriptProgress>? _prog;
        public PuppetEngine(CoreServices services, IProgress<ScriptProgress>? prog)
        {
            var commands = DiscoverCommands(services);
            _map = BuildCommandMap(commands);
            _prog = prog;
        }

        public PuppetResult Execute(string input)
        {
            var tokens = Tokenize(input);
            if (tokens.Count == 0) return PuppetResult.Ok("");

            var head = TokenizeCommandHead(tokens[0]);
            var args = tokens.Skip(1).ToList();

            // if the first argument is "help" or "?", we pass that to HelpCommand.

            if (args.Count > 0) if (args[0].ToLowerInvariant().Trim() == "help" || args[0].ToLowerInvariant().Trim() == "?")
            {
                try { return _map.GetValueOrDefault("help")!.Execute(TokenizeCommandHead("help"), Tokenize(tokens[0])); }
                catch (PuppetUserException ex) { return PuppetResult.Fail(ex.Message); }
                catch (Exception ex) { return PuppetResult.Fail($"Command '{head}' failed: '{ex.Message}'"); }
            }

            if (!_map.TryGetValue(head[0], out var cmd)) return PuppetResult.Fail($"Unknown command '{head[0]}', type 'help'.");
            try { return cmd.Execute(head, args); }
            catch (PuppetUserException ex) { return PuppetResult.Fail(ex.Message); }
            catch (Exception ex) { return PuppetResult.Fail($"Command '{string.Join('.', head)}' failed: {ex.Message}"); }
        }

        public PuppetResult ExecuteJson(string commandHead, string json)
        {
            // We don't like Jason so we're going to execute him. 
            var head = TokenizeCommandHead(commandHead);
            List<string> args = new();
            args.Add(json);
            if (!_map.TryGetValue(head[0], out var cmd)) return PuppetResult.Fail($"Unknown command '{head[0]}', type 'help'.");
            return cmd.Execute(head, args);
        }
        public bool RunScript(Script script, int previousSteps = 0)
        {
            int pv = previousSteps;
            int total = script.Statements.Count;
            _prog?.Report(new ScriptProgress(pv + 0, pv + 2, $"\n\n{new string('*', 50)}\n\nRunning script:\n\n{script.PrintInfo()}\n"));

            if (!ScriptValidateFormat(script)) return false;

            _prog?.Report(new ScriptProgress(pv + 1, pv + 2, $"\n\n{new string('*', 50)}\n\nFormat validated. Running script...\n"));

            foreach (ScriptStatement st in script.Statements)
            {
                try 
                { 
                    PuppetResult result = ExecuteJson(st.CommandHead, st.JsonPayload);
                    if (!result.Success)
                    {
                        _prog?.Report(new ScriptProgress(st.Index, total, $"Result for statement #{st.Index} returned as failed ('{st.CommandHead}', line {st.StartLine}):\n\"{result.Output}\"\nReturning."));
                        return false;
                    }
                    else _prog?.Report(new ScriptProgress(st.Index, total, $"Executed statement {st.PrintShortInfo()}"));
                }
                catch (PuppetUserException ex) 
                {
                    _prog?.Report(new ScriptProgress(st.Index - 1, total, $"Command #{st.Index} failed ('{st.CommandHead}', line {st.StartLine}), returning.\nPuppetUserException: \"{ex.Message}\"."));
                    return false;
                }
                catch (Exception ex) 
                {
                    _prog?.Report(new ScriptProgress(st.Index - 1, total, $"Command #{st.Index} failed ('{st.CommandHead}', line {st.StartLine}), returning.\nException: \"{ex.Message}\"."));
                    return false;
                }
                Thread.Sleep(2);
            }
            _prog?.Report(new ScriptProgress(pv + 2, pv + 2, $"\n\n{new string('*', 50)}\n\nCompleted successfully."));
            return true;
        }

        public bool RunScriptFromPath(string path)
        {
            _prog?.Report(new ScriptProgress(0, 3, $"\n\n{new string('*', 50)}"));
            Script script = ScriptParser.ParseFile(path, _prog);
            return RunScript(script, 1);
        }

        /// <summary>
        /// Runs the "TestJson" command in the given command. Use for input validation.
        /// </summary>
        public PuppetResult TestJson(string input)
        {
            var tokens = Tokenize(input);
            if (tokens.Count == 0) return PuppetResult.Ok("");

            var head = TokenizeCommandHead(tokens[0]);
            var args = tokens.Skip(1).ToList();

            if (!_map.TryGetValue(head[0], out var cmd)) return PuppetResult.Fail($"Unknown command '{head[0]}'");
            try { return cmd.TestJson(head, args); }
            catch (PuppetUserException ex) { return PuppetResult.Fail(ex.Message); }
            catch (Exception ex) { return PuppetResult.Fail($"Command '{head}' failed: '{ex.Message}'"); }
        }


        public bool ScriptValidateFormat(Script script)
        {
            int total = script.Statements.Count;
            int errors = 0;
            List<int> errorsIndex = new();
            _prog?.Report(new ScriptProgress(0, total, "Validating command format."));
            for (int i = 0; i < script.Statements.Count; i++)
            {
                ScriptStatement st = script.Statements[i];
                string command = string.Join(' ', st.CommandHead, st.JsonPayload);
                if (TestJson(command).Success)
                {
                    _prog?.Report(new ScriptProgress(i + 1, total, $"[PARSED] #{st.Index} (line {st.StartLine}): '{command.ToSingleLine().Unindent()}'".Truncate(150)));
                }
                else
                {
                    // Fail
                    errors++;
                    errorsIndex.Add(st.Index);                    
                    _prog?.Report(new ScriptProgress(i + 1, total, $"*[ERROR] #{st.Index} (line {st.StartLine}): '{command.ToSingleLine().Unindent()}'".Truncate(150)));
                }
                Thread.Sleep(2);
            }
            if (errors == 0)
            {
                _prog?.Report(new ScriptProgress(total, total, $"Success! No errors found."));
                return true;
            }
            else
            {
                StringBuilder sb = new();
                sb.AppendLine($"{errors} error(s) found:");
                foreach (int i in errorsIndex) sb.AppendLine($" - {script.Statements[i].PrintInfo}");
                _prog?.Report(new ScriptProgress(total, total, sb.ToString()));                
                return false;
            }
        }

        public bool ScriptValidateFormatPath(string path)
        {
            Script script = ScriptParser.ParseFile(path, _prog);
            return ScriptValidateFormat(script);
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
        bool ScriptValidateFormat(Script script);
        bool ScriptValidateFormatPath(string path);
        public bool RunScriptFromPath(string path);
    }

    public sealed record ScriptProgress(int ActionsDone, int ActionsTotal, string Message);
}
