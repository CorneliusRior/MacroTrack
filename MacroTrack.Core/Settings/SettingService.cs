using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Xml.Linq;

namespace MacroTrack.Core.Settings
{
    public class SettingsService
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _jsonOptions;

        public AppSettings Settings { get; private set; }

        public SettingsService(string filePath)
        {
            _filePath = filePath;

            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            Settings = Load();
        }

        private AppSettings Load()
        {
            if (!File.Exists(_filePath))
            {
                var defaults = new AppSettings();
                Save(defaults);
                return defaults;
            }

            string json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<AppSettings>(json, _jsonOptions) ?? new AppSettings();
        }

        public void Save()
        {
            Save(Settings);
        }

        private void Save(AppSettings settings)
        {
            string json = JsonSerializer.Serialize(settings, _jsonOptions);
            File.WriteAllText(_filePath, json);
        }
    }
}
