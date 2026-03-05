using MacroTrack.AppLibrary.Services;
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
using System.Windows.Shapes;

namespace MacroTrack.AppLibrary.Windows
{
    /// <summary>
    /// Interaction logic for DiaryEditWindow.xaml
    /// </summary>
    public partial class DiaryEditWindow : WindowBase
    {
        public DiaryEditWindow(CoreServices services, AppServices appServices, DiaryEntry entry) : base(services, appServices)
        {
            InitializeComponent();
            ControlDiaryEdit.Entry = entry;
            ControlDiaryEdit.Init(Services, AppServices);
            ControlDiaryEdit.RequestClose += r => Close();
            Log();
        }
    }
}
