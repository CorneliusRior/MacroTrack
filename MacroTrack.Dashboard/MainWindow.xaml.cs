using MacroTrack.AppLibrary.Controls;
using MacroTrack.AppLibrary.Graphs;
using MacroTrack.AppLibrary.Services;
using MacroTrack.AppLibrary.ViewModels;
using MacroTrack.AppLibrary.Windows.SettingsWindow;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Models;
using MacroTrack.Core.Services;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MacroTrack.Dashboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Core/CoreServices stuff:
        public CoreServices Services { get; private set; } = null!;
        public AppServices AppServices { get; private set; } = null!;
        private IMTLogger Logger = null!;
        private readonly MainWindowVM? _vm;

        // Event Subscriptions:
        private List<IDisposable> _subscriptions = new();

        public MainWindow()
        {
            
        }

        public MainWindow(CoreServices services, AppServices appServices) : this()
        {
            Services = services;
            Logger = services.Logger;
            AppServices = appServices;
            _vm = new MainWindowVM(Services, AppServices);
            DataContext = _vm;
            InitializeComponent();

            // Event Subscriptions:
            IDisposable _subSettingsChanged = AppServices.AppEvents.Subscribe<SettingsChanged>(_ =>
            {
                Print("SettingsChanged event announced");
                Log("SettiggsChanged event announced and detected.");
                RefreshAll(); // Given that this only calls 2 events which will also probably subscribe to it, we might not need it.
            });
            _subscriptions.Add(_subSettingsChanged);


            _vm.RequestPrint += text => Print(text);

            Log("Main window opened.", LogLevel.Info);
            WireUpControls();            
        }

        protected override void OnClosed(EventArgs e)
        {
            if (_vm != null) _vm.OnClose();
            foreach (IDisposable s in _subscriptions) s.Dispose(); // probably don't need to do this but you know.
            base.OnClosed(e);
        }

        // Set up controls:
        private void WireUpControls()
        {
            Summary.Init(Services, AppServices);
            DailyTasks.Init(Services, AppServices);
            FoodEntry.Init(Services, AppServices);
            WeightEntry.Init(Services, AppServices);
            DiaryEntry.Init(Services, AppServices);
            History.Init(Services, AppServices);
            Repl.Init(Services, AppServices);
            Repl.SubmitCommand += Repl_CommandHandler;            
        }

        // Log & REPL handling:
        private void Log(string message = "Called", LogLevel level = LogLevel.Debug, Exception? ex = null, [CallerMemberName] string caller = "")
        {
            Logger.Log(this, caller, level, message, ex);
        }

        /// <summary>
        /// Logs name and value of variables supplied in an anonymous object
        /// Format like LogVars(new{ a, b, c } [...] )
        /// </summary>
        /// <example>
        /// <code>
        /// LogVars(new{ a, b, c }, "Variables before");
        /// </code>
        /// </example>
        /// <param name="vars">An object whose public instances are logged</param>
        /// <param name="prefix">String which proceeds the variable listing in the log entry</param>
        /// <param name="caller">Automatically supplied member name of caller, ignore.</param>
        private void LogVars(object vars, string? prefix = null, [CallerMemberName] string caller = "")
        {
            Logger.LogVars(this, vars, caller, prefix);
        }

        private void Print(string text)
        {
            Repl.AppendLine(text);
        }

        private void RefreshAll()
        {
            //History.Refresh();
            //DailyTasks.Refresh();
        }

        private void Repl_CommandHandler(object? sender, string cmd)
        {
            Log($"Command \"{cmd}\"", LogLevel.Info);

            // Echo entered command:
            Print($"> {cmd}");

            // Handle (do externally):

            // This is a little test, disregard:
            if (cmd.ToLowerInvariant() == "hello world") Print("Hello!");
        }


        // Banner buttons:
        private void ButtonBannerYesterday_Click(object sender, RoutedEventArgs e)
        {
            Log();
            _vm?.OpenPreviousPeriodYesterday();
        }

        private void ButtonBannerPreviousPeriods_Click(object sender, RoutedEventArgs e)
        {
            Log();
            _vm?.OpenPreviousPeriodSelect();
        }

        private void BannerButtonSetGoal_Click(object sender, RoutedEventArgs e)
        {
            Log();
            _vm?.OpenGoalSet();
        }

        private void BannerButtonNewGoal_Click(object sender, RoutedEventArgs e)
        {
            Log();
            _vm?.OpenGoalNew();
        }

        private void ButtonBannerSettings_Click(object sender, RoutedEventArgs e)
        {
            Log();
            _vm?.OpenSettings();
        }

        private void ButtonBannerOpenLog_Click(object sender, RoutedEventArgs e)
        {
            Log();
            Logger.OpenLogFile();
        }

        private void ButtonBannerLogDirectory_Click(object sender, RoutedEventArgs e)
        {
            Log();
            Logger.OpenLogDir();
        }

        private void BannerButtonCheatDay_Click(object sender, RoutedEventArgs e)
        {
            Log();
            _vm?.DeclareCheatDayToday();
        }
    }
}