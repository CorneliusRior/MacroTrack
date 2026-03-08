using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MacroTrack.AppLibrary.Services
{
    public sealed class AppEvents : IAppEvents
    {
        private readonly ConcurrentDictionary<Type, List<Delegate>> _handlers = new();
        private readonly Dispatcher _dispatcher;

        public AppEvents()
        {
            _dispatcher = Application.Current?.Dispatcher ?? Dispatcher.CurrentDispatcher; // "CurrentDispatcher" means make a new dispatcher for the current application.
        }

        public IDisposable Subscribe<T>(Action<T> handler)
        {
            var list = _handlers.GetOrAdd(typeof(T), _ => new List<Delegate>());
            lock (list) list.Add(handler); // Lock makes it thread safe
            return new Unsubscriber(() => { lock (list) list.Remove(handler); });
        }

        public void Publish<T>(T message)
        {
            // Method called when one element wants to scream something to another element.            
            if (!_handlers.TryGetValue(typeof(T), out var list)) return; // Go through list of subscribers and see if there are any who will listen, return if no.
            Delegate[] snapshot; // Make references to respective methods
            lock (list) snapshot = list.ToArray(); // put Delegates into a list

            // Ensure UI-Thread delivery (We could just do invoke, but it's better to do it directly if that's possible, less overhead and conflicts).
            if (_dispatcher.CheckAccess()) foreach (var d in snapshot) ((Action<T>)d)(message);
            else _dispatcher.Invoke(() => { foreach (var d in snapshot) ((Action<T>)d)(message); });
        }        

        private sealed class Unsubscriber : IDisposable
        {
            // When we unsubscribe, we can get rid of the method "unsubscribe", useful for when you close windows or otherwise get rid of controls.
            private readonly Action _dispose;
            private bool _isDisposed;
            public Unsubscriber(Action dispose) => _dispose = dispose;
            public void Dispose()
            {
                if (_isDisposed) return;
                _isDisposed = true;
                _dispose();
            }
        }
    }
}
