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
    /// Interaction logic for DiaryEntryControl.xaml
    /// </summary>
    public partial class DiaryEntryControl : ControlBase
    {
        public DiaryEntryControl()
        {
            InitializeComponent();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (Services == null)
            {
                var ex = new Exception("Null Service");
                Log("Could not add entry.", LogLevel.Error);
                throw ex;
            }
            Services.diaryService.AddEntry(tbDiary.Text);
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            Log();
            tbDiary.Clear();
        }

        private void ButtonViewDiary_Click(object sender, RoutedEventArgs e)
        {
            Log();
        }
    }
}
