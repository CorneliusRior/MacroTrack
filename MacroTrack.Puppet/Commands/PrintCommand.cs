namespace MacroTrack.Puppet.Commands;

public class PrintCommand : ICommand
{
    public string Name => "print";
    public IReadOnlyList<string> Aliases => new[] { "p" };
    public string Description => "Prints text to the output.";
    public string Usage => "print \"Message\"";
    public string LongHelp => "Standard 'print' command. Make sure that the message is inside double quotations (\" \"), single quotations (' ') will not work. You can print single words without the quotations too. Any argument added after the message string will be ignored.";

    public string Execute(IReadOnlyList<string> args)
    {
        if (args.Count == 0) 
        {
            return ("Not enough arguments, usage: " + Usage);
        }

        return args[0];
    }
}