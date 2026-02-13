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
    /// Interaction logic for DiaryEditControl.xaml
    /// </summary>
    public partial class DiaryEditControl : ControlBase
    {
        private readonly DiaryEditVM _vm = new();
        public event Action<bool>? RequestClose;
        public DiaryEntry? Entry { get; set; }
        public DiaryEditControl()
        {
            InitializeComponent();
            DataContext = _vm;
            _vm.RequestClose += r => RequestClose?.Invoke(r);
        }

        public override void Init(CoreServices services, AppServices appServices)
        {
            base.Init(services, appServices);
            _vm.Init(services, appServices);
            if (Entry == null) throw new Exception("Null entry.");
            _vm.Populate(Entry);
        }

        protected override void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _vm.OnClose();
            base.OnUnloaded(sender, e);
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            RequestClose?.Invoke(false);
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (Entry == null) MessageBox.Show("Error: Null entry, cannot edit.", "Null entry", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                MessageBoxResult response = MessageBox.Show($"Delete Diary Entry #{Entry.Id}?\nThis cannot be undone.", "Edit Diary Entry", MessageBoxButton.YesNo);
                if (response == MessageBoxResult.Yes) { _vm.Delete(Entry); }
            }
        }

        private void buttonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (Entry == null) MessageBox.Show("Error: Null entry, cannot edit.", "Null entry", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                MessageBoxResult response = MessageBox.Show($"Apply specified edits to Diary Entry #{Entry.Id}?\nThis cannot be undone.", "Edit Diary Entry", MessageBoxButton.YesNo);
                if (response == MessageBoxResult.Yes) { _vm.Edit(Entry); }
            }            
        }
    }
}
