using MacroTrack.Core.Logging;
using MacroTrack.Core.Services;
using MacroTrack.Core.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MacroTrack.BasicApp.Forms
{
    public partial class Settings : Form
    {
        private CoreServices Services;
        private IMTLogger _logger;
        public event EventHandler<string>? RequestPrint;
        public event EventHandler<string>? RequestPrintInline;
        public event EventHandler? RequestRefresh;
        private bool _isLoading = false;
        private AppSettings _default;

        public Settings(CoreServices services)
        {
            InitializeComponent();
            Services = services;
            _logger = services.Logger;
            _default = new AppSettings();
            SetByAppSettings(Services.SettingsService.Settings);            
        }

        protected void Print(string text)
        {
            RequestPrint?.Invoke(this, text);
        }

        protected void PrintInline(string text)
        {
            RequestPrintInline?.Invoke(this, text);
        }

        private void Log(string message = "Called.", LogLevel level = LogLevel.Debug, Exception? ex = null, [CallerMemberName] string caller = "")
        {
            _logger.Log(this, nameof(caller), level, message, ex);
        }

        private void SetByAppSettings(AppSettings settings)
        {
            Log();
            if (_isLoading == true) return;
            _isLoading = true;
            Log("Entered");

            // Weight format:
            try { SetWeightFormat(settings.WeightMode); }
            catch (Exception ex)
            {
                Log("Error, must be integer, 0, 1, or 2, or BasicApp is outdated. Reverting to default.", LogLevel.Warning, ex);
                SetWeightFormat(_default.WeightMode);
            }

            // Logging, UI:
            try { SetLogUILevel(settings.LogUILevel); }
            catch (Exception ex)
            {
                Log("Error, must be either 'Debug', 'Info', 'Warning', or 'Error', or BasicApp is outdated. Reverting to default", LogLevel.Warning, ex);
                SetLogUILevel(_default.LogUILevel);
            }

            // Logging, File:
            try { SetLogFileLevel(settings.LogFileLevel); }
            catch (Exception ex)
            {
                Log("Error, must be either 'Debug', 'Info', 'Warning', or 'Error', or BasicApp is outdated. Reverting to default", LogLevel.Warning, ex);
                SetLogFileLevel(_default.LogUILevel);
            }
        }

        private void SetDefault()
        {
            SetWeightFormat(_default.WeightMode);
            SetLogUILevel(_default.LogUILevel);
            SetLogFileLevel(_default.LogFileLevel);
        }

        private void SetWeightFormat(int weightMode)
        {
            Log($"weightMode = {weightMode}");
            switch (weightMode)
            {
                case 0:
                    {
                        rbWFKg.Select();
                        break;
                    }
                case 1:
                    {
                        rbWFLbs.Select();
                        break;
                    }
                case 2:
                    {
                        rbWFSt.Select();
                        break;
                    }
                default:
                    {
                        throw new Exception($"Invalid WeightMode '{weightMode}'.");
                    }
            }
        }

        private void SetLogUILevel(LogLevel logUILevel)
        {
            Log($"logUILevel = {logUILevel}");
            switch (logUILevel)
            {
                case LogLevel.Debug:
                    {
                        rbREPLDebug.Select();
                        break;
                    }
                case LogLevel.Info:
                    {
                        rbREPLInfo.Select();
                        break;
                    }
                case LogLevel.Warning:
                    {
                        rbREPLWarning.Select();
                        break;
                    }
                case LogLevel.Error:
                    {
                        rbREPLError.Select();
                        break;
                    }
                default:
                    {
                        throw new Exception($"Invalid LogLevel '{logUILevel}'.");
                    }
            }
        }

        private void SetLogFileLevel(LogLevel logFileLevel)
        {
            Log($"logFileLevel = {logFileLevel}");
            switch (logFileLevel)
            {
                case LogLevel.Debug:
                    {
                        rbLogLogDebug.Select();
                        break;
                    }
                case LogLevel.Info:
                    {
                        rbLogLogInfo.Select();
                        break;
                    }
                case LogLevel.Warning:
                    {
                        rbLogLogWarning.Select();
                        break;
                    }
                case LogLevel.Error:
                    {
                        rbLogLogError.Select();
                        break;
                    }
                default:
                    {
                        throw new Exception($"Invalid LogLevel '{logFileLevel}'.");
                    }
            }
        }

        private void buttonDefault_Click(object sender, EventArgs e)
        {
            Log();
            SetDefault();
        }

        private void buttonRevert_Click(object sender, EventArgs e)
        {
            Log();
            SetByAppSettings(Services.SettingsService.Settings);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Log("Cancelled, closing", LogLevel.Info);
            Close();
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            // There's probably a better way to do it but I'm just going to do a lot of if statements man.
            Log($"Changing settings, presently they look like this: WeightMode = '{Services.SettingsService.Settings.WeightMode}', UILogLevel = '{Services.SettingsService.Settings.LogUILevel}', FileLogLevel = '{Services.SettingsService.Settings.LogFileLevel}'", LogLevel.Info);

            // Weight:
            if (rbWFKg.Checked) Services.SettingsService.Settings.WeightMode = 0;
            if (rbWFLbs.Checked) Services.SettingsService.Settings.WeightMode = 1;
            if (rbWFSt.Checked) Services.SettingsService.Settings.WeightMode = 2;

            // UILogLevel:
            if (rbREPLDebug.Checked) Services.SettingsService.Settings.LogUILevel = LogLevel.Debug;
            if (rbREPLInfo.Checked) Services.SettingsService.Settings.LogUILevel = LogLevel.Info;
            if (rbREPLWarning.Checked) Services.SettingsService.Settings.LogUILevel = LogLevel.Warning;
            if (rbREPLError.Checked) Services.SettingsService.Settings.LogUILevel = LogLevel.Error;

            // FileLogLevel:
            if (rbLogLogDebug.Checked) Services.SettingsService.Settings.LogFileLevel = LogLevel.Debug;
            if (rbLogLogInfo.Checked) Services.SettingsService.Settings.LogFileLevel = LogLevel.Info;
            if (rbLogLogWarning.Checked) Services.SettingsService.Settings.LogFileLevel = LogLevel.Warning;
            if (rbLogLogError.Checked) Services.SettingsService.Settings.LogFileLevel = LogLevel.Error;

            Log($"Changed settings, now they look like this: WeightMode = '{Services.SettingsService.Settings.WeightMode}', UILogLevel = '{Services.SettingsService.Settings.LogUILevel}', FileLogLevel = '{Services.SettingsService.Settings.LogFileLevel}'", LogLevel.Info);
            Services.SettingsService.Save();
            _logger.UILevel = Services.SettingsService.Settings.LogUILevel;
            _logger.FileLevel = Services.SettingsService.Settings.LogFileLevel;

            RequestRefresh?.Invoke(this, EventArgs.Empty);
            Close();
        }
    }
}
