using MacroTrack.AppLibrary.Services;
using MacroTrack.AppLibrary.ViewModels;
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
    /// Interaction logic for AddPresetControl.xaml
    /// </summary>
    public partial class AddPresetControl : ControlBase
    {
        private readonly AddPresetVM _vm = new();
        public event Action<bool>? RequestClose;

        public static readonly DependencyProperty InWindowProperty = DependencyProperty.Register(
            nameof(InWindow),
            typeof(bool),
            typeof(AddPresetControl), 
            new PropertyMetadata(false)
        );
        public bool InWindow
        {
            get => (bool)GetValue(InWindowProperty);
            set => SetValue(InWindowProperty, value);
        }
        public AddPresetControl()
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
            _vm.OnClose();
            base.OnUnloaded(sender, e);
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            RequestClose?.Invoke(true);
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            _vm.Clear();
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            Log("Add button clicked");
            bool addresult = _vm.Add();
            if (addresult) RequestClose?.Invoke(true);
        }
    }
}
