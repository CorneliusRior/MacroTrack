using MacroTrack.AppLibrary.Windows;
using MacroTrack.AppLibrary.Windows.SettingsWindow;
using MacroTrack.Core.DataModels;
using MacroTrack.Core.Models;
using MacroTrack.Core.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace MacroTrack.AppLibrary.Services
{
    internal class WindowService : IWindowService
    {
        /* How to make a new window "Cool":
         *  - Make a new WPF Window in the namespace MacroTrack.AppLibrary.Windows (Directory AppLibrary/Windows), name it "CoolWindow.xaml".
         *  - Leave it blank (or w/ message textblock). Change the opening and closing tags from "Window" to "local:WindowBase".
         *  - Open CoolWindow.xaml.cs, change it from "CoolWindow : Window" to "CoolWindow : WindowBase".
         *  - Give constructor "(CoreServices services, AppServices appServices) : base(services, appServices)".
         *  - Go to MacroTrack.AppLibrary.WindowType, add the window name, keeping it in alphabetical order except for "Settings".
         *  - Go to MacroTrack.AppLibrary.WindowService (here), scroll to the bottom and create a new method "CreateCoolWindow", copying format of the others.
         *  - Go to the function CreateWindow() and add it to the switch, copying format of others.
         *  - Now, anywhere with access to AppServices can call this window with AppServices.WindowService.Show(WindowType.Cool).
         *  - If you want it to have parameters, have CreateWindow() pass it, check that it's the right type, then pass it. 
         *  - Make a new class in the same directory as Cool.xaml called CoolVM.cs if it does anything other than just host a single control.
         */
        private readonly CoreServices _services;
        private readonly AppServices _appServices;
        private readonly Dispatcher _dispatcher;

        // Single-instance windows, only one allowed at a tie.
        private readonly Dictionary<WindowType, Window> _singleInstance = new();
        private static readonly HashSet<WindowType> _singleInstanceTypes = new()
        {
            WindowType.Settings,
            WindowType.AddPreset,
            WindowType.GoalNew,
            WindowType.GoalSet
        };

        public WindowService(CoreServices services, AppServices appServices)
        {
            _services = services;
            _appServices = appServices;
            _dispatcher = Application.Current?.Dispatcher ?? Dispatcher.CurrentDispatcher;
        }

        public void Show(WindowType type, object? parameter = null)
        {
            InvokeOnUI(() =>
            {
                if (_singleInstanceTypes.Contains(type))
                {
                    if (_singleInstance.TryGetValue(type, out var existing) && existing.IsVisible)
                    {
                        // If this window you want is a single instance only type, and there's one already open, show it front and centre.
                        existing.Activate();
                        existing.Focus();
                        return;
                    }

                    var w = CreateWindow(type, parameter);
                    TrackSingleInstance(type, w);
                    w.Show();
                    return;
                }
                else
                {
                    // Else allow multiple windows:
                    var w = CreateWindow(type, parameter);
                    w.Show();
                }
            });
        }

        public bool? ShowDialog(WindowType type, object? parameter = null)
        {
            return InvokeOnUI(() =>
            {
                var w = CreateWindow(type, parameter);
                return w.ShowDialog();
            });
        }

        private Window CreateWindow(WindowType type, object? parameter)
        {
            var o = GetActiveWindow();
            return type switch
            {
                WindowType.Settings => CreateSettingsWindow(o),
                WindowType.AddPreset => CreateAddPresetWindow(o),
                WindowType.DiaryView => CreateDiaryViewWindow(o),
                WindowType.DiaryEdit => CreateDiaryEditWindow(o, parameter),
                WindowType.FoodLogEdit => CreateFoodLogEditWindow(o, parameter),
                WindowType.GoalNew => CreateGoalNewWindow(o),
                WindowType.GoalSet => CreateGoalSetWindow(o),
                WindowType.PresetManage => CreatePresetManageWindow(o),
                WindowType.PreviousPeriod => CreatePreviousPeriodWindow(o, parameter),
                WindowType.PreviousPeriodSelect => CreatePreviousPeriodSelectWindow(o, parameter),
                WindowType.TaskManage => CreateTaskManageWindow(o),
                _ => throw new NotSupportedException($"Unknown window type '{type}'")
            };
        }        

        private void TrackSingleInstance(WindowType type, Window w)
        {
            _singleInstance[type] = w;
            w.Closed += (_, _) => _singleInstance.Remove(type);
        }

        private Window? GetActiveWindow()
        {
            // Basically whichever you're on, that's the parent window, unless there's none in which case it's main window, unless that doesn't exist in which case it's null.
            return Application.Current?.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive) ?? Application.Current?.MainWindow;
        }

        private void InvokeOnUI(Action action)
        {
            if (_dispatcher.CheckAccess()) action();
            else _dispatcher.Invoke(action);
        }

        private T InvokeOnUI<T>(Func<T> action)
        {
            if (_dispatcher.CheckAccess()) return action();
            return _dispatcher.Invoke(action);
        }

        private Window CreateSettingsWindow(Window? owner)
        {
            return new SettingsWindow(_services, _appServices) { Owner = owner };            
        }

        private Window CreateAddPresetWindow(Window? owner)
        {
            return new AddPresetWindow(_services, _appServices) { Owner = owner };
        }

        private Window CreateDiaryViewWindow(Window? owner)
        {
            return new DiaryViewWindow(_services, _appServices) { Owner = owner };
        }

        private Window CreateDiaryEditWindow(Window? owner, object? parameter) 
        {
            if (parameter is DiaryEntry entry)
            {
                return new DiaryEditWindow(_services, _appServices, entry) { Owner = owner };
            }
            else throw new InvalidOperationException();
        }
        private Window CreateFoodLogEditWindow(Window? owner, object? parameter)
        {
            if (parameter is FoodEntry entry)
            {
                return new FoodLogEditWindow(_services, _appServices, entry) { Owner = owner };
            }
            else throw new InvalidOperationException();
        }

        private Window CreateGoalNewWindow(Window? owner)
        {
            return new GoalNewWindow(_services, _appServices);
        }

        private Window CreateGoalSetWindow(Window? owner)
        {
            return new GoalSetWindow(_services, _appServices);
        }

        private Window CreatePresetManageWindow(Window? owner)
        {
            return new PresetManageWindow(_services, _appServices);
        }

        private Window CreatePreviousPeriodWindow(Window? owner, object? parameter)
        {
            if (parameter is TimePeriod timePeriod)
            {
                return new PreviousPeriodWindow(_services, _appServices, timePeriod) { Owner = owner };
            }
            else throw new InvalidOperationException();
        }

        private Window CreatePreviousPeriodSelectWindow(Window? owner, object? parameter)
        {
            if (parameter is ICommand openCommand)
            {
                return new PreviousPeriodSelectWindow(_services, _appServices, openCommand) { Owner = owner };
            }
            else throw new InvalidOperationException();
        }

        private Window CreateTaskManageWindow(Window? owner) 
        {
            return new TaskManageWindow(_services, _appServices);
        }
    }
}
