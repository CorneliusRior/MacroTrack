using MacroTrack.Core.Services;
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

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(CoreServices services) : this()
        {
            //InitializeComponent();
            Services = services;
            Services.Logger.Log(this, "Constructor", Core.Logging.LogLevel.Info, "Main window opened");
        }
    }
}