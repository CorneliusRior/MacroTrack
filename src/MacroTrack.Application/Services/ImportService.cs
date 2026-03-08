using System;
using System.Windows;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.AppLibrary.Services
{
    /// <summary>
    /// We could make this part of services but like, genuinely why would you do that?
    /// </summary>
    internal static class ImportFunctions
    {
        /// <summary>
        /// Used to import a txt file from assets. If you ever need to use txt files, put them in the assets folder and import it from this. This will also work with subdirectories. If you want to use other files, make another more general method, but you could probably still use this.
        /// </summary>
        /// <param name="fileName"></param>
        /// <example>string bannerText = ImportAssetTxt("banner");</example>
        public static string ImportAssetTxt(string fileName) => File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Assets", $"{fileName}.txt"));
    }
}
