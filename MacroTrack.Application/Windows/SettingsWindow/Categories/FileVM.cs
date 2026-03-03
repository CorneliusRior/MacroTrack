using MacroTrack.AppLibrary.Commands;
using MacroTrack.Core.Infrastructure;
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

            ViewLogCommand = new RelayCommand(() => Logger.OpenLogFile());
            ViewLogDirCommand = new RelayCommand(() => Logger.OpenLogDir());
            ExportDataCommand = new RelayCommand(() => ExportData());
            OpenDataFileCommand = new RelayCommand(() => Paths.OpenDataDir());
        }

        /// <summary>
        /// Allows you to export data to excel. We do use exportService, which we might use for backups (we haven't done that yet, we will, rewriting much of Infrastructure/Paths &c.). It is kept in here due to "SaveFileDialogue" needing to be in a WPF environment, originally we tried to put this in export services actually, but it didn't work.
        /// </summary>
        public void ExportData()
        {
            var dlg = new SaveFileDialog
            {
                Filter = "Excel Workbook (*.xlsx)|*.xlsx",
                FileName = "MTDataExport.xlsx"
            };
            if (dlg.ShowDialog() == true) Services.fileService.ExportToExcel(dlg.FileName);
        }
    }
}
