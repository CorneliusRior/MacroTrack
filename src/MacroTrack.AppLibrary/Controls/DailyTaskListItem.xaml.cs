using MacroTrack.Core.Models;

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
    /// Interaction logic for DailyTaskListItem.xaml
    /// </summary>
    public partial class DailyTaskListItem : UserControl
    {
        public static readonly DependencyProperty TaskProperty = DependencyProperty.Register(
            nameof(Task),
            typeof(DailyTask),
            typeof(DailyTaskListItem),
            new PropertyMetadata(null)
        );
        public DailyTask Task
        {
            get => (DailyTask)GetValue(TaskProperty);
            set => SetValue(TaskProperty, value);
        }

        public DailyTaskListItem()
        {
            InitializeComponent();
        }
    }
}
