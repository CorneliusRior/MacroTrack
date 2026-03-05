using MacroTrack.AppLibrary.Services;
using MacroTrack.AppLibrary.ViewModels;
using MacroTrack.Core.Models;
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
    /// Interaction logic for FoodEntryEditControl.xaml
    /// </summary>
    public partial class FoodLogEditControl : ControlBase
    {
        private readonly FoodLogEditVM _vm = new();
        public event Action<bool>? RequestClose;
        public FoodEntry? Entry { get; set; }
        public FoodLogEditControl()
        {
            InitializeComponent();
            DataContext = _vm;
            _vm.RequestClose += r => RequestClose?.Invoke(r);
        }

        public override void Init(CoreServices services, AppServices appServices)
        {
            base.Init(services, appServices);
            _vm.Init(services, appServices);
            if (Entry == null) throw new Exception("Null Entry");
            _vm.Populate(Entry);
        }

        protected override void OnUnloaded(object sender, RoutedEventArgs e)
        {
            Log("This is to affirm that 'OnUnLoaded is being called'");
            _vm.OnClose();
            base.OnUnloaded(sender, e);
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            RequestClose?.Invoke(false);
        }

        private void buttonEdit_Click(object sender, RoutedEventArgs e)
        {
            _vm.Edit();
        }
    }
}
