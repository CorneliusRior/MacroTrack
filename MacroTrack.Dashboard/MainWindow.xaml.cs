using MacroTrack.AppLibrary.Controls;
using MacroTrack.AppLibrary.Graphs;
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
        private IMTLogger Logger = null!;
        private readonly MainWindowVM? _vm;

        public MainWindow()
        {
            
        }

        public MainWindow(CoreServices services) : this()
        {
            Services = services;
            Logger = services.Logger;
            _vm = new MainWindowVM(Services);
            DataContext = _vm;
            InitializeComponent();

            _vm.RequestPrint += text => Print(text);
            _vm.RequestOpenSettings += () =>
            {
                var w = new SettingsWindow(Services) { Owner = this };
                w.RequestRefresh += () =>
                {
                    RefreshAll();
                    _vm.RefreshVM();
                };
                w.Show();
            };
            _vm.RequestRefreshAll += RefreshAll;


            Log("Main window opened.", LogLevel.Info);
            WireUpControls();            
        }

        // Set up controls:
        private void WireUpControls()
        {
            Summary.Init(Services);
            FoodEntry.Init(Services);
            WeightEntry.Init(Services);
            DiaryEntry.Init(Services);
            History.Init(Services);
            History.RequestRefresh += () => _vm?.RefreshSummaryCommand.Execute(null);
            Repl.Init(Services);
            Repl.SubmitCommand += Repl_CommandHandler;            
        }

        // Log & REPL handling:
        private void Log(string message = "Called", LogLevel level = LogLevel.Debug, Exception? ex = null, [CallerMemberName] string caller = "")
        {
            Logger.Log(this, caller, level, message, ex);
        }

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
            History.Refresh();
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
        }

        private void ButtonBannerPreviousPeriods_Click(object sender, RoutedEventArgs e)
        {
            Log();
        }

        private void BannerButtonSetGoal_Click(object sender, RoutedEventArgs e)
        {
            Log();
        }

        private void BannerButtonNewGoal_Click(object sender, RoutedEventArgs e)
        {
            Log();
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
    }
}