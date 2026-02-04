using MacroTrack.Core.Infrastructure;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Models;
using MacroTrack.Core.Repositories;
using MacroTrack.Core.Services;
using MacroTrack.Core.Settings;
using MacroTrack.Puppet;
using System.IO;

Console.WriteLine("MacroTrack 8.0: Console Interface (using Puppet).");
/*
var dbPath = FindDBPath();
var connString = $"Data Source={dbPath}";
var logPath = FindLogPath();
string logFile = Path.Combine(logPath, $"MTLog_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt");
DeleteOldLogs(20);
var _logger = new MTLogger(logFile);
var _context = new CoreContext(_logger);
var settingsPath = FindSettingsPath();
var settingsService = new SettingsService(settingsPath);

var _services = new CoreServices(connString, _context, settingsService);
*/

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

/*
static string FindSolutionRoot()
{
    var dir = new DirectoryInfo(AppContext.BaseDirectory);
    while (dir != null && !Directory.Exists(Path.Combine(dir.FullName, "Data"))) dir = dir.Parent;
    if (dir == null) throw new DirectoryNotFoundException("No");
    return dir.FullName;
}

static string FindAppDataDir()
{
    var baseDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    return Path.Combine(baseDir, "MacroTrack");
}

static string FindDBPath()
{
    var dir = Path.Combine(FindAppDataDir(), "data");
    Directory.CreateDirectory(dir);
    string fileName;
#if DEBUG
    fileName = "MacroTrack.debug.db";
#else
            fileName = "MacroTrack.db";
#endif
    return Path.Combine(dir, fileName);
}

static string FindLogPath()
{
    var dir = Path.Combine(FindAppDataDir(), "logs");
    Directory.CreateDirectory(dir);
    return dir;
}

static void DeleteOldLogs(int amount)
{
    var dir = FindLogPath();
    var files = new DirectoryInfo(dir).GetFiles("MTLog_*.txt").OrderByDescending(f => f.CreationTime).ToList();
    foreach (var f in files.Skip(amount)) f.Delete();
}

static string FindSettingsPath()
{
    var dir = Path.Combine(FindAppDataDir(), "config");
    Directory.CreateDirectory(dir);
    return Path.Combine(dir, "settings.json");
}
*/
