using MacroTrack.Puppet2.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.Puppet2
{
    public sealed record PuppetResult(bool Success, string Output, RestartArgs? RestartArgs = null)
    {
        public static PuppetResult Ok(string output) => new(true, output);
        public static PuppetResult Fail(string output) => new(false, output);
        public static PuppetResult FailHelp(CommandHelp help, FailHelpType failType = FailHelpType.Default)
        {
            string output;
            output = failType switch
            {
                FailHelpType.Default => $"{string.Join('.', help.Head)}",
                FailHelpType.NoSubCommand => $"{string.Join('.', help.Head)} requires a subcommand. {(help.SubCommands is null ? "" : $"Available subcommands: {{ \"{string.Join($"\", \"")}\" }}")}",
                FailHelpType.NoArgs => $"{string.Join('.', help.Head)} requires arguments.",
                _ => throw new ArgumentOutOfRangeException()

            };
            if (help.Aliases is not null) output += $"\n{"Aliases", -15} {{ \"{string.Join($"\", \"", help.Aliases)}\" }}";
            output += $"\n{"Usage:",-15} {help.Usage}\n{"Description:",-15} {help.Description}";
            if (!string.IsNullOrWhiteSpace(help.Example)) output += $"\n{"Example:",-15} {help.Example}";
            if (!string.IsNullOrWhiteSpace(help.LongDescription)) output += $"\n{help.LongDescription}";

            return new(false, output);
        }

        public static PuppetResult RequestRestart(string message = "", string caption = "", string printOutput = "Restart Requested") => new(true, printOutput, RestartArgs.Request(message, caption));
        public static PuppetResult ForceRestart() => new(true, "Restarting...", RestartArgs.Force());
    }

    public enum FailHelpType
    {
        Default,
        NoSubCommand,
        NoArgs
    }

    public sealed class PuppetUserException : Exception
    {
        public string? SourceCommand { get; }
        public PuppetUserException(string message, string? sourceCommand = null) : base(message) 
        {
            SourceCommand = sourceCommand;
        }
    }

    public sealed record RestartArgs(RestartRequestType RequestType = RestartRequestType.Default, string Message = "", string Caption = "")
    {
        public static RestartArgs Request(string message = "", string caption = "") => new(RestartRequestType.RequestRestart, message, caption);
        public static RestartArgs Force() => new(RestartRequestType.ForceRestart);
    }


    // This is just added in case you want to put in more arguments, it literally does nothing.
    public enum RestartRequestType
    {
        Default,
        RequestRestart,
        ForceRestart
    }
}
