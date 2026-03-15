using MacroTrack.AppLibrary.Converters;
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
    /// Interaction logic for DailyTaskManageControl.xaml
    /// </summary>
    public partial class DailyTaskManageControl : ControlBase
    {
        private readonly DailyTaskManageVM _vm = new();
        public event Action<bool>? RequestClose;
        public DailyTaskManageControl()
        {
            InitializeComponent();
            DataContext = _vm;
            _vm.RequestClose += r => RequestClose?.Invoke(r);
        }

        public override void Init(CoreServices services, AppServices appServices)
        {
            base.Init(services, appServices);
            _vm.Init(services, appServices);

            TaskHistoryMatrix History = Services!.taskService.GetHistory();
            LoadHistoryMatrix(History);
        }

        protected override void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _vm.OnClose();
            base.OnUnloaded(sender, e);
        }

        private void LoadHistoryMatrix(TaskHistoryMatrix matrix)
        {
            dgHistory.Columns.Clear();
            dgHistory.ItemsSource = matrix.Rows;

            dgHistory.Columns.Add(new DataGridTextColumn
            {
                Header = "Date",
                Binding = new Binding(nameof(TaskHistoryRow.Date))
                {
                    StringFormat = "yyyy-MM-dd"
                },
                Width = new DataGridLength(140)
            });

            foreach (DailyTask task in matrix.Tasks)
            {
                dgHistory.Columns.Add(new DataGridTextColumn
                {
                    Header = task.Name,
                    Binding = new Binding($"Completed[{task.Id}]")
                    {
                        Converter = (IValueConverter)FindResource("BoolToX")
                    },
                    Width = new DataGridLength(50),
                    ElementStyle = (Style)FindResource("TextBlock.TaskHistory")
                });
            }
        }
    }
}
