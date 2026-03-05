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
    public partial class AddLogEntryControl : ControlBase
    {
        private readonly AddLogEntryVM _vm = new();
        
        public AddLogEntryControl()
        {
            InitializeComponent();
            DataContext = _vm;
        }

        protected override void OnUnloaded(object sender, RoutedEventArgs e)
        {
            base.OnUnloaded(sender, e);
            _vm.OnClose();
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            var msg = tbLog.Text.Trim();

            if (!string.IsNullOrWhiteSpace(msg))
            {
                Log(msg, LogLevel.Info);
                _vm.LastAdded = $"Last added: {msg}";
                tbLog.Text = string.Empty;
            }
            else
            {
                Log("(No text)", LogLevel.Warning);
            }
        }
    }
}
            

