using MacroTrack.AppLibrary.Controls;
using MacroTrack.AppLibrary.ViewModels;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Services;
using MacroTrack.Core.Models;
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

        // ViewModels:
        private readonly FoodEntryVM _foodVM = new();

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(CoreServices services) : this()
        {
            Services = services;
            Logger = services.Logger;
            Log("Main window opened.", LogLevel.Info);
            WireUpControls();            
        }

        // Set up controls:
        private void WireUpControls()
        {
            /*
            AddLogEntry.Services = Services;
            AddLogEntry.Logger = Logger;

            DiaryEntry.Services = Services;
            DiaryEntry.Logger = Logger;

            Repl.Services = Services;
            Repl.Logger = Logger;
            */
            WeightEntry.Init(Services);
            DiaryEntry.Init(Services);
            Repl.Init(Services);
            Repl.SubmitCommand += Repl_CommandHandler;

            /*
            FoodEntry.Services = Services;
            FoodEntry.Logger = Logger;
            _foodVM.Services = Services;
            _foodVM.Logger = Logger;
            */
            FoodEntry.Init(Services);

            //RefreshPresets();
        }

        // Log & REPL handling:
        private void Log(string message = "Called", LogLevel level = LogLevel.Debug, Exception? ex = null, [CallerMemberName] string caller = "")
        {
            Logger.Log(this, caller, level, message, ex);
        }

        private void Print(string text)
        {
            Repl.AppendLine(text);
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
        }

        private void ButtonBannerLightDark_Click(object sender, RoutedEventArgs e)
        {
            Log();
            
            ThemeManager.ToggleLightDark();
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