using MacroTrack.AppLibrary.Commands;
using MacroTrack.AppLibrary.Resources;
using MacroTrack.AppLibrary.Services;
using MacroTrack.AppLibrary.Windows.SettingsWindow.Categories;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Services;
using MacroTrack.Core.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MacroTrack.AppLibrary.Windows.SettingsWindow
{
    public class SettingsWindowVM
    {
        public CoreServices Services;
        public IMTLogger Logger;
        public AppServices AppServices;

        public AppSettings SettingsEditable { get; }
        public ObservableCollection<CategoryVMBase> Categories { get; } = new();
        public CategoryVMBase SelectedCategory { get; set; }

        public ICommand ApplyCommand { get; }
        public ICommand OKCommand { get; }
        public ICommand CancelCommand { get; }

        private List<IDisposable> _subscriptions = new();


        public event Action<bool>? RequestClose;
        
        public SettingsWindowVM(CoreServices services, AppServices appServices)
        {            
            Services = services;
            Logger = services.Logger;
            AppServices = appServices;
            Log();
            SettingsEditable = Services.SettingsService.Settings.Clone();


            AddCategories();
            SelectedCategory = Categories[0];

            ApplyCommand = new RelayCommand(Apply);
            OKCommand = new RelayCommand(() => { Apply(); RequestClose?.Invoke(true); });
            CancelCommand = new RelayCommand(() => RequestClose?.Invoke(false));
        }

        public void OnClose()
        {
            foreach (IDisposable s in _subscriptions) s.Dispose();
        }

        private void AddCategories()
        {
            Log();
            /*
            How to add new categories:
             - Make a new WPF Usercontrol in Categories folder, copying format of others: Expenders &c.
             - Make a corresponding VM of type CategoryVMBase, and a constructor like:
                    "public CategoryVM(AppSettings settings) : base("CategoryTitle", settings)
             - Add it to SettingsWindowVM.cs "AddCategories()"
             - Add it to <MTWin:WindowBase.Resources> in SettingsWindow.xaml
             */
            Categories.Add(new GeneralVM(SettingsEditable));
            Categories.Add(new FileVM(SettingsEditable, Services));
            Categories.Add(new GraphSettingsVM(SettingsEditable));
            Categories.Add(new LoggingVM(SettingsEditable));
            Categories.Add(new FormattingVM(SettingsEditable));
        }

        public void Apply()
        {
            Log();
            Services.SettingsService.Set(SettingsEditable);
            AppServices.AppEvents.Publish(new SettingsChanged());
        }

        public void SetSelectedToDefault()
        {
            Log();
            SelectedCategory.SetToDefault();
        }

        public void SetSelectedToCurrent()
        {
            Log();
            SelectedCategory.SetToCurrent();
        }

        private void Log(string message = "Called", LogLevel level = LogLevel.Debug, Exception? ex = null, [CallerMemberName] string caller = "")
        {
            Logger?.Log(this, caller, level, message, ex);
        }
    }
}
