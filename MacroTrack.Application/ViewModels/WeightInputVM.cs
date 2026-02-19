using MacroTrack.AppLibrary.Graphs;
using MacroTrack.AppLibrary.Services;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Models;
using MacroTrack.Core.Services;
using MacroTrack.Core.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MacroTrack.AppLibrary.ViewModels
{
    public class WeightInputVM : ViewModelBase
    {
        public WeightInputVM() { }

        public override void Init(CoreServices services, AppServices appServices)
        {
            base.Init(services, appServices);
            EventSubscribe(AppServices!.AppEvents.Subscribe<SettingsChanged>(_ => UpdateSettings()));
            EventSubscribe(AppServices!.AppEvents.Subscribe<WeightLogChanged>(_ => DrawGraph()));
            UpdateSettings();
            DrawGraph();
            TimeNow();
        }

        private void UpdateSettings()
        {
            Format = Services!.SettingsService.Settings.WeightFormat;
            GraphSettings = Services!.SettingsService.Settings.GraphSettings;
            UnitLabel = Format.ShortString();
            Convert();
        }

        private WeightFormat _format;
        public WeightFormat Format
        {
            get => _format;
            set
            {
                if (_format == value) return;
                _format = value;
                OnPropertyChanged();
            }
        }

        private string? _unitLabel;
        public string? UnitLabel
        {
            get => _unitLabel;
            set
            {
                if (_unitLabel == value) return;
                _unitLabel = value; 
                OnPropertyChanged();
            }
        }

        private string? _conversionString;
        public string? ConversionString
        {
            get => _conversionString;
            set
            {
                if (_conversionString == value) return;
                _conversionString = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _time;
        public DateTime? Time
        {
            get => _time;
            set
            {
                if (_time == value) return;
                _time = value;
                OnPropertyChanged();
                DateTimeRequire(nameof(Time), Time);
            }
        }

        private double? _weightInput;
        public double? WeightInput
        {
            get => _weightInput;
            set
            {
                Log();
                if (_weightInput == value) return;
                _weightInput = value;
                Convert();
                OnPropertyChanged();                
            }
        }

        public double? WeightKg { get; private set; }
        public double? WeightLbs { get; private set; }
        public double? WeightSt { get; private set; }

        // Graphing variables:
        private GraphSettings? _graphSettings;
        public GraphSettings? GraphSettings
        {
            get => _graphSettings;
            set
            {
                if (_graphSettings == value) return;
                _graphSettings = value;
                OnPropertyChanged();
            }
        }

        private DateTime _graphStartTime;
        public DateTime GraphStartTime
        {
            get => _graphStartTime;
            set
            {
                if (_graphStartTime == value) return;
                _graphStartTime = value;
                OnPropertyChanged();
            }
        }

        private DateTime _graphEndTime;
        public DateTime GraphEndTime
        {
            get => _graphEndTime;
            set
            {
                if (_graphEndTime == value) return;
                _graphEndTime = value;
                OnPropertyChanged();
            }
        }

        private IReadOnlyList<PlotSeries>? _graphSeriesSet;
        public IReadOnlyList<PlotSeries>? GraphSeriesSet
        {
            get => _graphSeriesSet;
            set
            {
                if (_graphSeriesSet == value) return;
                _graphSeriesSet = value;
                OnPropertyChanged();
            }
        }

        public void TimeNow()
        {
            Time = DateTime.Now;
        }

        public void Clear()
        {
            Log();
            if (Time ==  null) Time = DateTime.Now;            
            WeightInput = null;
            ClearAllErrors();
        }

        public void ClearAllErrors()
        {
            ClearError(nameof(Time));
            ClearError(nameof(WeightInput));
        }

        private void Convert()
        {
            if ( WeightInput == null)
            {
                WeightKg = 0;
                WeightLbs = 0;
                WeightSt = 0;
            }
            else
            {
                double w = WeightInput.Value; 
                WeightKg = Format.ConvertTo(WeightFormat.Kg, w);
                WeightLbs = Format.ConvertTo(WeightFormat.Lbs, w);
                WeightSt = Format.ConvertTo(WeightFormat.St, w);
            }
            
            UpDateConversionString();
            LogVars(new {WeightKg, WeightLbs, WeightSt, Format});
        }

        private void UpDateConversionString()
        {
            if (Format == WeightFormat.Kg)  ConversionString = $"( {WeightLbs:0.#}lbs, {WeightSt:0.#}st )";
            if (Format == WeightFormat.Lbs) ConversionString = $"( {WeightKg:0.#}kg, {WeightSt:0.#}st )";
            if (Format == WeightFormat.St)  ConversionString = $"( {WeightKg:0.#}kg, {WeightLbs:0.#}lbs )";
        }

        public void Add()
        {
            Log();
            bool ok = true;
            ok &= DateTimeRequire(nameof(Time), Time);
            ok &= NumericRequire(nameof(WeightInput), WeightInput);
            if (!ok) return;

            DateTime time = Time!.Value;
            double weight = Format.ConvertTo(WeightFormat.Kg, WeightInput!.Value);

            try
            {
                if (Services == null) throw new Exception("Null Services");
                WeightEntry entry = Services.weightLogService.AddEntry(time, weight);
                Log($"Added entry #{entry.Id}", LogLevel.Info);
                if (AppServices == null) throw new Exception("NullAppServices");
                AppServices.AppEvents.Publish(new WeightLogChanged());
                Clear();
            }
            catch (Exception ex) { Log("Could not add entry", LogLevel.Error, ex); }
        }

        public void DrawGraph()
        {
            if (Services == null) throw new Exception("Null Services");
            GraphStartTime = DateTime.Today.AddDays(-Services.SettingsService.Settings.WeightGraphLength);
            GraphEndTime = DateTime.Today.AddDays(1);
            List<WeightEntry> weightLog = Services.dataService.GetWeightEntries(GraphStartTime.AddDays(-5), GraphEndTime); // AddDays(-5) to have data before.
            IReadOnlyList<DataPoint> weightDataPoints = ConvertToDataPoints(weightLog);

            // use data to make trend line, draw that before WeightSeries.

            PlotSeries WeightSeries = new()
            {
                SeriesType = SeriesType.LineContinuous,
                DataPoints = weightDataPoints,
                SeriesColor = SeriesColor.LineSeriesBrush1,
                ShowTrendline = true,
                ShowTrendLineStdDev = true
            };

            IReadOnlyList<PlotSeries> seriesSet = new List<PlotSeries> { WeightSeries };
            GraphSeriesSet = seriesSet;
        }

        private IReadOnlyList<DataPoint> ConvertToDataPoints(List<WeightEntry> weightLog)
        {
            if (Services == null) throw new Exception("Null Services");
            List<DataPoint> dataPoints = new();
            foreach (WeightEntry entry in weightLog) dataPoints.Add(new DataPoint { Time = entry.Time, Value = entry.Weight });
            dataPoints = dataPoints.OrderBy(w => w.Time).ToList();
            return dataPoints;
        }
    }
}
