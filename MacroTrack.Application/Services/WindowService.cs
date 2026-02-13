using MacroTrack.AppLibrary.Windows;
using MacroTrack.AppLibrary.Windows.SettingsWindow;
using MacroTrack.Core.Models;
using MacroTrack.Core.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MacroTrack.AppLibrary.Services
{
    internal class WindowService : IWindowService
    {
        private readonly CoreServices _services;
        private readonly AppServices _appServices;
        private readonly Dispatcher _dispatcher;

        // Single-instance windows, only one allowed at a tie.
        private readonly Dictionary<WindowType, Window> _singleInstance = new();
        private static readonly HashSet<WindowType> _singleInstanceTypes = new()
        {
            WindowType.Settings,
            WindowType.AddPreset
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
                WindowType.FoodLogEdit => CreateFoodLogEditWindow(o),
                WindowType.TaskView => CreateTaskViewWindow(o),
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
        private Window CreateFoodLogEditWindow(Window? owner) { throw new NotImplementedException(); }
        private Window CreateTaskViewWindow(Window? owner) { throw new NotImplementedException(); }
    }
}
