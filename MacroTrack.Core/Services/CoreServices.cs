using MacroTrack.Core.Repositories;
using System.Reflection;

namespace MacroTrack.Core.Services;

public sealed class CoreServices
{
    public DataService dataService { get; }
    public DiaryService diaryService { get; }
    public FoodLogService foodLogService { get; }
    public GoalService goalService { get; }
    public TaskService taskService { get; }
    public PresetService presetService { get; }
    public WeightLogService weightLogService { get; }

    // ...

    public event EventHandler<string> RequestPrint;
    public event EventHandler<string> RequestPrintInline;

    public CoreServices(string connectionString)
    {
        dataService = new DataService(new FoodLogRepo(connectionString), new GoalRepo(connectionString), new WeightLogRepo(connectionString));
        diaryService = new DiaryService(new DiaryRepo(connectionString));
        foodLogService = new FoodLogService(new FoodLogRepo(connectionString), new PresetRepo(connectionString));
        goalService = new GoalService(new GoalRepo(connectionString));
        presetService = new PresetService(new PresetRepo(connectionString));
        taskService = new TaskService(new TaskRepo(connectionString));
        weightLogService = new WeightLogService(new WeightLogRepo(connectionString));
        
        dataService.RequestPrint += (sender, text) => Print(sender!, text);
        diaryService.RequestPrint += (sender, text) => Print(sender!, text);
        foodLogService.RequestPrint += (sender, text) => Print(sender!, text);
        goalService.RequestPrint += (sender, text) => Print(sender!, text);
        presetService.RequestPrint += (sender, text) => Print(sender!, text);
        taskService.RequestPrint += (sender, text) => Print(sender!, text);
        weightLogService.RequestPrint += (sender, text) => Print(sender!, text);

        dataService.RequestPrintInline += (_, text) => PrintInline(text);
        diaryService.RequestPrintInline += (_, text) => PrintInline(text);
        foodLogService.RequestPrintInline += (_, text) => PrintInline(text);
        goalService.RequestPrintInline += (_, text) => PrintInline(text);
        presetService.RequestPrintInline += (_, text) => PrintInline(text);
        taskService.RequestPrintInline += (_, text) => PrintInline(text);
        weightLogService.RequestPrintInline += (_, text) => PrintInline(text);
    }

    private void Print(object sender, string text)
    {
        RequestPrint?.Invoke(this, $"{sender}.{text}");
    }

    private void PrintInline(string text)
    {
        RequestPrintInline?.Invoke(this, text);
    }
}