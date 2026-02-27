using MacroTrack.AppLibrary.Services;
using MacroTrack.AppLibrary.ViewModels;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
    }
}
