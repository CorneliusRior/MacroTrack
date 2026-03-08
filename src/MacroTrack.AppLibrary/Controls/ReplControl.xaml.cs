using MacroTrack.AppLibrary.Services;
using MacroTrack.AppLibrary.ViewModels;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Services;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace MacroTrack.AppLibrary.Controls
{
    /// <summary>
    /// Interaction logic for ReplControl.xaml
    /// </summary>
    public partial class ReplControl : ControlBase
    {
        private readonly ReplVM _vm = new();
        public event EventHandler<string>? SubmitCommand;
        private List<string> _history = new();
        private int _historyIndex = -1;
        private string _current = "";
        public ReplControl()
        {
            InitializeComponent();
            DataContext = _vm;
        }

        public override void Init(CoreServices services, AppServices appServices)
        {
            base.Init(services, appServices);
            _vm.Init(services, appServices);
            DispatcherTimer t = new() { Interval = TimeSpan.FromSeconds(1) };
            t.Tick += (_, _) =>
            {
                t.Stop();
                RollBanner();
            };
            t.Start();
        }

        protected override void OnUnloaded(object sender, RoutedEventArgs e)
        {
            base.OnUnloaded(sender, e);
            _vm.OnClose();
        }

        public void AppendLine(string line)
        {
            tbOutput.AppendText(line + Environment.NewLine);
            tbOutput.ScrollToEnd();
        }

        private void ButtonEnter_Click(object sender, RoutedEventArgs e)
        {
            Log();
            Submit();
        }

        private void tbInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Submit();
                e.Handled = true;
            }
            if (e.Key == Key.Up)
            {
                if (_historyIndex == 0) return;
                if (_historyIndex == _history.Count) _current = tbInput.Text;
                if (_historyIndex > 0) _historyIndex--;
                tbInput.Text = _history[_historyIndex];
                tbInput.CaretIndex = tbInput.Text.Length;
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Down)
            {
                if (_historyIndex < _history.Count) _historyIndex++;
                if (_historyIndex == _history.Count) tbInput.Text = _current;
                else tbInput.Text = _history[_historyIndex];
                tbInput.CaretIndex = tbInput.Text.Length;
                e.Handled = true;
                return;
            }
        }

        private void Submit()
        {
            var input = tbInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(input)) return;
            _history.Add(input);
            _historyIndex = _history.Count;
            _current = "";
            SubmitCommand?.Invoke(this, input);
            tbInput.Clear();
        }

        public void PrintTimedMultiLine(string[] lines, double seconds = 0.025)
        {
            int i = 0;
            DispatcherTimer t = new() { Interval = TimeSpan.FromSeconds(seconds) };
            t.Tick += (_, _) =>
            {
                if (i >= lines.Length)
                {
                    t.Stop();
                    return;
                }
                AppendLine(lines[i]);
                i++;
            };
            t.Start();
        }

        public void RollBanner() => PrintTimedMultiLine( ImportFunctions.ImportAssetTxt("banner").SplitToLines());
         
    }

    /// <summary>
    /// If you can think of somewhere more suitable to put this, put it there. For now it lives here because I need to use it here.
    /// </summary>
    public static class StringFunctions
    {
        public static string[] SplitToLines(this string text) => text.Replace("\r\n", "\n").Replace("\r", "\n").Split("\n", StringSplitOptions.None);
    }
        
}
