using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MacroTrack.Puppet2.Scripting
{
    public static class ScriptParser
    {
        public static Script ParseFile(string path)
        {
            p("Got here.");
            string[] lines = File.ReadAllLines(path);
            bool inBlockComment = false;
            int statementIndex = 0; 
            List<ScriptStatement> statements = new();
            p($"Got here, lines.length='{lines.Length}'");

            for (int i = 0; i < lines.Length; i++)
            {
                p($"Got here, i='{i}'");
                // Filter comments & empty:
                string t = lines[i].Trim();
                if (t.StartsWith('#'))
                {
                    inBlockComment = !inBlockComment;
                    continue;
                }
                if (inBlockComment) continue;
                if (t.StartsWith("//")) continue;
                if (string.IsNullOrWhiteSpace(lines[i])) continue;

                // Now we presume that this is a JSON:
                string commandHead;
                string jsonStart = "";
                int startLine = i + 1;
                int bindex = lines[i].IndexOf('{');
                p("Got here.");
                if (bindex >= 0)
                {
                    commandHead = lines[i][..bindex].Trim();
                    jsonStart = lines[i][bindex..].Trim();
                }
                else commandHead = lines[i].Trim();
                if (string.IsNullOrWhiteSpace(commandHead)) throw new Exception($"Missing command head at line ({startLine}).");

                // Start TryParseJson loop:
                StringBuilder sb = new();
                sb.Append(jsonStart);
                while (true)
                {
                    p("Got here.");
                    if (sb.ToString().TryParseJson()) break;
                    i++;
                    if (i >= lines.Length) throw new Exception($"Cannot parse JSON of statement #{statementIndex}, probably failed to close statement (no \"}}\").");
                    if (!string.IsNullOrWhiteSpace(lines[i])) sb.AppendLine(lines[i]);
                }

                statements.Add(new ScriptStatement(statementIndex, startLine, commandHead, sb.ToString().Trim()));
                statementIndex++;
            }
            p($"Got here. Statements.Count='{statements.Count}'");
            // Create metadata:
            if (!statements[0].CommandHead.Equals("ScriptMetadata", StringComparison.OrdinalIgnoreCase)) throw new Exception("First statement is not marked ScriptMetadata");
            p("Got here.");
            ScriptMetaData metaData = JsonSerializer.Deserialize<ScriptMetaData>(statements[0].JsonPayload) ?? throw new Exception("Could not parse metadata.");
            p("Got here.");
            return new Script(metaData, statements.Skip(1).ToList());
        }

        private static bool TryParseJson(this string json)
        {
            try
            {
                using var _ = JsonDocument.Parse(json);
                return true;
            }
            catch (JsonException) { return false; }
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

    public sealed record Script(
        ScriptMetaData MetaData,
        IReadOnlyList<ScriptStatement> Statements)
    {
        public string PrintInfo() => MetaData.PrintInfo() + $" {Statements.Count} statements.";
        public string PrintFullInfo()
        {
            StringBuilder sb = new();
            sb.AppendLine(MetaData.PrintInfo());
            sb.AppendLine($"\nAll Statements:");
            foreach (ScriptStatement s in Statements) sb.AppendLine($"\n{s.PrintInfo()}");
            return sb.ToString();
        }
    }

    public sealed record ScriptMetaData(
        string Format,
        string Name,
        string Author,
        DateTime Created)
    {
        public string PrintInfo() => $"{Name}.\nAuthor: {Author}, Created: {Created.ToString("G")}\nFormat: '{Format}'.";
    }

    public sealed record ScriptStatement(
        int Index,
        int StartLine,
        string CommandHead,
        string JsonPayload)
    {
        public string PrintInfo() => $"Statement #{Index} (line {StartLine}):\n{CommandHead} {JsonPayload.Replace("\n", " ").Replace("\r", " ")}";
    }


}
