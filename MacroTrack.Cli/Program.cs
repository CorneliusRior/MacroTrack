using System.IO;
using MacroTrack.Core.Models;
using MacroTrack.Core.Repositories;
using MacroTrack.Core.Services;
using MacroTrack.Puppet;

Console.WriteLine("MacroTrack 8.0: Console Interface (using Puppet).");
var root = FindSolutionRoot();
var dbPath = Path.Combine(root, "Data", "MacroTrack.debug.db");
var connString = $"Data Source={dbPath}";

var _services = new CoreServices(connString);
var _puppet = new Puppet(_services);

while (true)
{
    Console.Write("> ");
    string input = Console.ReadLine().Trim();
    switch(input)
    {
        case "exit":
            Console.WriteLine("Goodbye");
            Environment.Exit(0);
            break;
        default:
            Console.WriteLine(_puppet.Execute(input));
            break;
    }
}

static string FindSolutionRoot()
{
    var dir = new DirectoryInfo(AppContext.BaseDirectory);
    while (dir != null && !Directory.Exists(Path.Combine(dir.FullName, "Data"))) dir = dir.Parent;
    if (dir == null) throw new DirectoryNotFoundException("No");
    return dir.FullName;
}

