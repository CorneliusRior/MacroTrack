using MacroTrack.Core.Logging;
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
        public CoreServices Services { get; private set; } = null!;
        private IMTLogger Logger = null!;

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(CoreServices services) : this()
        {
            //InitializeComponent();
            Services = services;
            Logger = services.Logger;
            Log("Main window opened.", LogLevel.Info);
        }

        private void Log(string message = "Called", LogLevel level = LogLevel.Debug, Exception? ex = null, [CallerMemberName] string caller = "")
        {
            Logger.Log(this, caller, level, message, ex);
        }

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
    }
}