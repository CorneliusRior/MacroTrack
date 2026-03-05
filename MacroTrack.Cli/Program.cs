using MacroTrack.Core.Infrastructure;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Models;
using MacroTrack.Core.Repositories;
using MacroTrack.Core.Services;
using MacroTrack.Core.Settings;
using MacroTrack.Puppet;
using System.IO;

Console.WriteLine("MacroTrack 8.0: Console Interface (using Puppet).");

// Get paths & apply:
// Database:
string dbPath = Paths.FindDBPath();
string connString = $"Data Source ={dbPath}";

// Logger:
string logPath = Paths.FindLogPath();
string logFile = Path.Combine(logPath, $"MTLog_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt");
var logger = new MTLogger(logFile, "Puppet CLI");

// Settings:
string settingsPath = Paths.FindSettingsPath();
var settingsService = new SettingsService(settingsPath, logger);
settingsService.Apply(settingsService.Settings);

// Application of some settings:
Paths.DeleteOldLogs(settingsService.Settings.LogRetainAmount);

// Create context, then CoreServices
var context = new CoreContext(connString, logger, settingsService);
var Services = new CoreServices(context);
var _puppet = new Puppet(Services);

while (true)
{
    Console.Write("> ");
    string input = Console.ReadLine()!.Trim();
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
