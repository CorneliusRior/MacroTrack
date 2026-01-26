using MacroTrack.Core.Services;

namespace MacroTrack.BasicApp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        

        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            var root = FindSolutionRoot();
            var dbPath = Path.Combine(root, "Data", "MacroTrack.debug.db");
            var connString = $"Data Source={dbPath}";

            var _services = new CoreServices(connString);

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1(_services));
        }

        static string FindSolutionRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !Directory.Exists(Path.Combine(dir.FullName, "Data"))) dir = dir.Parent;
            if (dir == null) throw new DirectoryNotFoundException("No");
            return dir.FullName;
        }
    }
}