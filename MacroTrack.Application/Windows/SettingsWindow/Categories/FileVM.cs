using MacroTrack.AppLibrary.Commands;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Services;
using MacroTrack.Core.Settings;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MacroTrack.AppLibrary.Windows.SettingsWindow.Categories
{
    class FileVM : CategoryVMBase
    {
        public CoreServices Services;
        public IMTLogger Logger;
        public ICommand ViewLogCommand { get; }
        public ICommand ViewLogDirCommand { get; }
        public ICommand ExportDataCommand { get; }
        public ICommand OpenDataFileCommand { get; }

        public FileVM(AppSettings settings, CoreServices services) : base("File", settings)
        {
            Services = services;
            Logger = services.Logger;

            ViewLogCommand = new RelayCommand(() => ViewLog());
            ViewLogDirCommand = new RelayCommand(() => ViewLogDir());
            ExportDataCommand = new RelayCommand(() => ExportData());
            OpenDataFileCommand = new RelayCommand(() => OpenDataFile());
        }

        public void ViewLog()
        {
            Logger.OpenLogFile();
        }

        public void ViewLogDir()
        {
            Logger.OpenLogDir();
        }

        public void ExportData()
        {
            var dlg = new SaveFileDialog
            {
                Filter = "Excel Workbook (*.xlsx)|*.xlsx",
                FileName = "MTDataExport.xlsx"
            };
            if (dlg.ShowDialog() == true) Services.exportService.ExportToExcel(dlg.FileName);
        }

        private void OpenDataFile()
        {

        }
    }
}
