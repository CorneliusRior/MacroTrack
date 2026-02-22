using MacroTrack.Core.Repositories;
using MacroTrack.Core.Logging;
using System.Reflection;
using System.Runtime.CompilerServices;
using MacroTrack.Core.Infrastructure;
using MacroTrack.Core.Settings;

namespace MacroTrack.Core.Services;

/// <summary>
/// Collection of all services needed to interact with data, should be able to use all functions in Core with this service.
/// </summary>
public sealed class CoreServices
{
    public IMTLogger Logger { get; }
    public event EventHandler<LogMessage>? MessageLogged;

    public SettingsService SettingsService { get; }

    public DiaryRepo diaryRepo { get; }
    public ExportRepo exportRepo { get; }
    public FoodLogRepo foodLogRepo { get; }
    public GoalRepo goalRepo { get; }
    public PresetRepo presetRepo { get; }
    public TaskRepo taskRepo { get; }
    public WeightLogRepo weightLogRepo { get; }

    public DataService dataService { get; }
    public DiaryService diaryService { get; }
    public ExportService exportService { get; }
    public FoodLogService foodLogService { get; }
    public GoalService goalService { get; }
    public TaskService taskService { get; }
    public PresetService presetService { get; }
    public WeightLogService weightLogService { get; }


    public CoreServices(CoreContext ctx)
    {
        string conn = ctx.ConnString;

        Logger = ctx.Logger;
        Logger.MessageLogged += (sender, msg) => MessageLogged?.Invoke(sender, msg);

        SettingsService = ctx.Settings;

        diaryRepo = new DiaryRepo(conn, ctx);
        exportRepo = new ExportRepo(conn, ctx);
        foodLogRepo = new FoodLogRepo(conn, ctx);
        goalRepo = new GoalRepo(conn, ctx);
        presetRepo = new PresetRepo(conn, ctx);
        taskRepo = new TaskRepo(conn, ctx);
        weightLogRepo = new WeightLogRepo(conn, ctx);

        dataService = new DataService(foodLogRepo, goalRepo, weightLogRepo, ctx);
        diaryService = new DiaryService(diaryRepo, ctx);
        exportService = new ExportService(exportRepo, ctx);
        foodLogService = new FoodLogService(foodLogRepo, presetRepo, ctx);
        goalService = new GoalService(goalRepo, ctx);
        taskService = new TaskService(taskRepo, ctx);
        presetService = new PresetService(presetRepo, ctx);
        weightLogService = new WeightLogService(weightLogRepo, ctx);

        Log("CoreServices Built");
    }

    private void Log(string message, LogLevel level = LogLevel.Debug, Exception? ex = null, [CallerMemberName] string caller = "")
    {        
        Logger.Log(this, caller, level, message, ex);
    }
}