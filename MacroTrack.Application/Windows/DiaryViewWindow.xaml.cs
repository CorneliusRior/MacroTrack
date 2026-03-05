using MacroTrack.AppLibrary.Services;
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
    /// Interaction logic for DiaryView.xaml
    /// </summary>
    public partial class DiaryViewWindow : WindowBase
    {
        public DiaryViewWindow(CoreServices services, AppServices appServices) : base(services, appServices)
        {
            InitializeComponent();            
            ControlDiary.Init(Services, AppServices);            
        }
    }
}
