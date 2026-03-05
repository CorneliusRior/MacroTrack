using MacroTrack.Core.Models;
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
        public static Script ParseFile(string path, IProgress<ScriptProgress>? prog = null)
        {
            string[] lines = File.ReadAllLines(path);
            int total = lines.Length + 1;
            bool inBlockComment = false;
            int statementIndex = 0;
            List<ScriptStatement> statements = new();

            string fileName = Path.GetFileName(path);
            prog?.Report(new ScriptProgress(0, total, $"Starting, attempting to parse file '{fileName}' ({lines.Length} lines):"));

            for (int i = 0; i < lines.Length; i++)
            {
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
                Thread.Sleep(2);
                string commandHead;
                string jsonStart = "";
                int startLine = i + 1;
                int bindex = lines[i].IndexOf('{');
                
                if (bindex >= 0)
                {
                    commandHead = lines[i][..bindex].Trim();
                    jsonStart = lines[i][bindex..].Trim();
                }
                else commandHead = lines[i].Trim();
                if (string.IsNullOrWhiteSpace(commandHead)) throw new Exception($"Missing command head at line ({startLine}).");

                prog?.Report(new ScriptProgress(i, total, $"Found statement #{statementIndex} '{commandHead}': '{lines[i]}'"));

                // Start TryParseJson loop:
                StringBuilder sb = new();
                sb.Append(jsonStart);
                while (true)
                {
                    prog?.Report(new ScriptProgress(i, total, $"    + line {i}: '{lines[i]}'"));
                    if (sb.ToString().TryParseJson()) break;
                    i++;
                    if (i >= lines.Length) throw new Exception($"Cannot parse JSON of statement #{statementIndex} (line {startLine}): Current line exceeds document length, probably failed to close statement (no \"}}\").\n\nCurrently looks like:\n{sb.ToString()}");
                    if (!string.IsNullOrWhiteSpace(lines[i])) sb.AppendLine(lines[i]);
                    Thread.Sleep(2);
                }

                statements.Add(new ScriptStatement(statementIndex, startLine, commandHead, sb.ToString().Trim()));
                prog?.Report(new ScriptProgress(i, total, $"Added statement #{0} '{commandHead}' (lines {startLine}-{i})"));
                statementIndex++;                
            }

            // Create metadata:
            prog?.Report(new ScriptProgress(total -1, total, "Done, parsing metadata."));
            if (!statements[0].CommandHead.Equals("ScriptMetadata", StringComparison.OrdinalIgnoreCase)) throw new Exception("First statement is not marked ScriptMetadata");
            ScriptMetaData metaData = JsonSerializer.Deserialize<ScriptMetaData>(statements[0].JsonPayload) ?? throw new Exception("Could not parse metadata.");
            prog?.Report(new ScriptProgress(total, total, "Done"));
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
        public string PrintInfo() => $"Statement #{Index} (line {StartLine}):\n{CommandHead} {JsonPayload.Replace("\n", " ").Replace("\r", " ").ToSingleLine().Unindent()}";
        public string PrintShortInfo() => $"#{Index} (line {StartLine}): {CommandHead} {JsonPayload.ToSingleLine().Unindent()}".Truncate(150);
    }


}
