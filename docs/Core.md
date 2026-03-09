# MacroTrack.Core

MacroTrack.Core is the core Class Library for MacroTrack, which handles data structuring, saving and loading, app settings and logging. Everything in Core is accessed through `CoreServices`.

## Structure

Everything in Core is accessed through the class `CoreServices`, which contains 11 services. 7 of these are for specific data types and follow a structure: `CoreServices ⇆ Service ⇆ Repo ⇆ Database`, the remaining 4 are unique. These are:
- `Logger`
- `SettingsService`
- `DataService`
- `FileService`

### `IMTLogger` Logger
`IMTLogger` is the MacroTrack Logging system. It writes a given message along with namespace and method in a log file which is created when the application is opened.

Log files are kept in the log directory `AppData\Local\MacroTrack\logs`. This directory can also be accessed in the app through Settings > File > Logging > Open Log Directory, or Settings > Logging > File > Open Log Directory. 

`MTLogger` is an implementation of `IMTLogger` containing the methods:
- `Log()`: Adds an entry to the log file.
    - Can be called with just `Log()`, and it will enter a log entry with the message "Called". 
    - `LogLevel` is an Enum to designate different levels of importance. Options are: [ `Debug`, `Info`, `Warning`, `Error` ].
- `LogVars()`: Adds and entry to the log files listing specified variables.

Do not call these methods directly, instead, paste this method into the base class of any class which will use either:

```csharp
protected void Log(string message = "Called", LogLevel level = LogLevel.Debug, Exception? ex = null, [CallerMemberName] string caller = "")
{
    Logger.Log(this, caller, level, message, ex);
}

/// <summary>
/// Logs name and value of variables supplied in an anonymous object.
/// Format like LogVars(new{ a, b, c } [...] )
/// </summary>
/// <example>
/// <code>
/// LogVars(new{ a, b, c }, "Variables before");
/// </code>
/// </example>
/// <param name="vars">An object whose public instances are logged.</param>
/// <param name="prefix">String which proceeds the variable listing in the log entry.</param>
/// <param name="caller">Automatically supplied member name of caller, ignore.</param>
protected void LogVars(object vars, string? prefix = null, [CallerMemberName] string caller = "")
{
    Logger?.LogVars(this, vars, caller, prefix);
}
```

### SettingsService

Settings are stored in a settings.Json file in `AppData\Local\MacroTrack\config`. If it does not exist SettingsService will create one. When the program is loaded it will parse this file into an `AppSettings` object. These can be accessed with `CoreServices.settingService.Settings`. When adding new settings, make sure to add it as a property and also add the property to the `Clone()` method below.

### DataService

`DataService` is a service which uses existing repos to provide longer term/timeseries data. 

### FileService
`FileService` can back up data, load data, and create new empty databases. Presently this is only used by Puppet2 and by extention AppLibrary and Dashboard.

This does have its own `FileRepo` like the "Other Services" listed below, but it is seperated from them due to it not handling a datatype.

### Other Services:

There is another service and corresponding repo for the following data types:
- Diary
- Food Log
- Goal
- Preset
- Daily Tasks
- Weight Log

### Rules
Each of these owns a repo which has access to the database through SQLite. No SQLite code should be written anywhere in this program other than here (apart from quick applications like `MT5toMTScript`), and only services inside of `CoreServices` should be allowed to access these repos. Services should be an implementation of `ServiceBase` and repos should be an implementation of `RepoBase`. Logic, conversion &c. should be kept to services as much as possible. Copy implementation of existing services.

## Implementation

To set up Core, on application startup, create Logger, create SettingsService, apply settings, create CoreContext, then CoreServices, and pass this to the application.

Here is an example from `MacroTrack.Dashboard` (`AppServices` is specific to `MacroTrack.AppLibrary` applications):

```csharp
protected override void OnStartup(StartupEventArgs e)
{
    base.OnStartup(e);

    // Setup: //

    // Logger:
    string logPath = Paths.FindLogPath();
    string logFile = Path.Combine(logPath, $"MTLog_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt");
    var logger = new MTLogger(logFile, this.GetType().Namespace);

    // Settings:
    string settingsPath = Paths.FindSettingsPath();
    var settingsService = new SettingsService(settingsPath, logger);
    settingsService.Apply(settingsService.Settings);

    // Application of some settings:
    Paths.DeleteOldLogs(settingsService.Settings.LogRetainAmount);
            
    // Database:
    string dbPath = settingsService.GetStartupDatabase() ?? Paths.FindDBPath();
    string connString = $"Data Source={dbPath}";

    // Create context, then CoreServices:
    var context = new CoreContext(connString, logger, settingsService);
    Services = new CoreServices(context);

    // Create AppServices:
    AppServices = new AppServices(Services);

    // Show Window:
    var mainWindow = new MainWindow(Services, AppServices);
    mainWindow.Show();            
}
```

Internally, `CoreServices` is generally just called "Services", it is placed before anything else including `AppServices`.