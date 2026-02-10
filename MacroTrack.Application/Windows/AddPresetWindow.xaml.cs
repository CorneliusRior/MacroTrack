using MacroTrack.AppLibrary.Services;
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
using System.Windows.Shapes;

namespace MacroTrack.AppLibrary.Windows
{
    /// <summary>
    /// Interaction logic for AddPresetWindow.xaml
    /// </summary>
    public partial class AddPresetWindow : Window
    {
        private CoreServices Services;
        private IMTLogger Logger;
        private AppServices AppServices;

        /*
        public AddPresetWindow()
        {
            InitializeComponent();
        }*/

        public AddPresetWindow(CoreServices services, AppServices appServices)
        {
            InitializeComponent();
            Services = services;
            Logger = services.Logger;
            AppServices = appServices;
        }
    }
}
