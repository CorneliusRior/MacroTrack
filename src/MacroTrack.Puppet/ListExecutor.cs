namespace MacroTrack.Puppet;

/* The idea of this is to be able ot put in text files, or maybe we can call them .mt files, which will consist of a list of commands which will be run one at a time. I'm sure you could come up with a couple of uses for this. You could use this to input a series of presets, or goals, we might even use this for the dreaded conversion process. I don't know, it's cool, also somewhat non-functional. We might rewrite everything that's in here but for the time being this is a placeholder. */

public class ListExecutor
{
    private readonly Puppet _puppet;

    public ListExecutor(Puppet puppet)
    {
        _puppet = puppet;
    }

    public void ExecuteList(List<string> commandList, Action<string>? onOutput = null)
    {
        foreach (var line in commandList)
        {
            var output = _puppet.Execute(line);
            if (!string.IsNullOrWhiteSpace(output)) onOutput?.Invoke(output);
        }
    }
}