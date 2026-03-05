using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MacroTrack.Dashboard
{
    public static class ApplicationLifeCycle
    {
        // Closes the program without asking you.
        public static void Close() => Application.Current.Shutdown();

        public static void Restart()
        {
            string exePath = Process.GetCurrentProcess().MainModule!.FileName!;
            Process.Start(new ProcessStartInfo
            {
                FileName = exePath,
                UseShellExecute = true
            });
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Makes a messagebox stating that the application will shut down, would you like it to restart, and also a cancel button.
        /// </summary>
        /// <param name="message">Printed at the top of the box before "Application will now shut down, would you like it to restart?"</param>
        /// <param name="caption">The title/caption of the messagebox. Default is "Restart Program?"</param>
        public static void RestartCloseCancel(string message = "", string? caption = null)
        {
            MessageBoxResult r = MessageBox.Show(
                messageBoxText: $"{(string.IsNullOrWhiteSpace(message) ? "Closing application, would you like to restart?" : message)}",
                caption: caption ?? "Restart Program?",
                MessageBoxButton.YesNoCancel
                );
            if (r == MessageBoxResult.Yes) Restart();
            if (r == MessageBoxResult.No) Close();
            // else they did cancel, and won't open.
        }
    }
}
