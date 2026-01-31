using MacroTrack.Core.Logging;
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
        public event EventHandler<string>? SubmitCommand;
        public ReplControl()
        {
            InitializeComponent();
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
        }

        private void Submit()
        {
            var input = tbInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(input)) return;
            SubmitCommand?.Invoke(this, input);
            tbInput.Clear();
        }
    }
}
