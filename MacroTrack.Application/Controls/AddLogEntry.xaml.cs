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
    /// Interaction logic for AddLogEntry.xaml
    /// </summary>
    public partial class AddLogEntryControl : UserControl
    {
        public CoreServices? Services { get; set; }
        public IMTLogger? Logger { get; set; }
        private readonly AddLogEntryWM _vm = new();
        public AddLogEntryControl()
        {
            InitializeComponent();
            DataContext = _vm;
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            var msg = tbLog.Text?.Trim();

            if (msg != null)
            {
                Logger?.Log(this, "Add", LogLevel.Info, msg);
                _vm.LastAdded = $"Last added: {msg}";
                tbLog.Text = string.Empty;
            }
            else
            {
                Logger?.Log(this, "Add", LogLevel.Warning, "No message :(");
            }
        }
    }
}
            

