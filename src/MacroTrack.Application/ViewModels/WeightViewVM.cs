using MacroTrack.AppLibrary.Commands;
using MacroTrack.AppLibrary.Graphs;
using MacroTrack.AppLibrary.Services;
using MacroTrack.Core.DataModels;
using MacroTrack.Core.Models;
using MacroTrack.Core.Services;
using MacroTrack.Core.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MacroTrack.AppLibrary.ViewModels
{
    public class WeightViewVM : ViewModelBase
    {
        public ObservableCollection<WeightEntry> Entries { get; } = new ObservableCollection<WeightEntry>();
        private readonly HashSet<WeightEntry> _selected = new();
        public IReadOnlyCollection<WeightEntry> Selected => _selected;

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

        private string? _timeFormat;
        public string? TimeFormat
        {
            get => _timeFormat;
            set
            {
                if (_timeFormat == value) return;
                _timeFormat = value;
                OnPropertyChanged();
            }
        }

        private TimePeriod _period;
        public TimePeriod Period
        {
            get => _period;
            set
            {
                _period = value;
                OnPropertyChanged();
            }
        }

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

        // Commands:
        public ICommand SelectionChangedCommand { get; }
        

        public WeightViewVM ()
        {
            _period = new TimePeriod(DateTime.Today, DateTime.Today.AddDays(1));
            SelectionChangedCommand = new RelayCommand<SelectionChanged>(p =>
            {
                if (p is null) return;
                SetSelected(p.entry, p.IsSelected);
            });
        }

        public override void Init(CoreServices services, AppServices appServices)
        {
            base.Init(services, appServices);
            EventSubscribe(AppServices!.AppEvents.Subscribe<SettingsChanged>(_ =>
            {
                UpdateSettings();
                Populate();
                DrawGraph();
            }));
            EventSubscribe(AppServices!.AppEvents.Subscribe<WeightLogChanged>(_ =>
            {
                Populate();
                DrawGraph();
            }));
            UpdateSettings();
            Populate();
            DrawGraph();
        }

        private void UpdateSettings()
        {
            Format = Services!.SettingsService.Settings.WeightFormat;
            TimeFormat = $"({Services.SettingsService.GetDTFormatShortString()}):";
            GraphSettings = Services.SettingsService.Settings.GraphSettings;
            UnitLabel = Format.ShortString();
            Log($"This is unit label rn: '{UnitLabel}'");
        }

        public void Populate()
        {
            if (Services == null) throw new Exception("Null Services");
            if (Period == null) return;
            List<WeightEntry> weightEntries;
            try
            {
                weightEntries = Services.weightLogService.FromTimes(Period.StartTime, Period.EndTime);
            }
            catch
            {
                return; // no entries probably.
            }
            
            // Convert if needs be:
            if (Format != WeightFormat.Kg)
            {
                foreach (WeightEntry entry in weightEntries) entry.Weight = WeightFormat.Kg.ConvertTo(Format, entry.Weight);
            }
            
            Entries.Clear();
            _selected.Clear();
            foreach (WeightEntry entry in weightEntries)
            {
                entry.Weight = Math.Round(entry.Weight, 1);
                Entries.Add(entry);
            }
        }

        public void SetSelected(WeightEntry entry, bool isSelected)
        {
            if (isSelected) _selected.Add(entry);
            else _selected.Remove(entry);
            string selectedList = $"Selected should have changed: '{(isSelected ? "Add" : "Remove")}' entry '#{entry.Id}', _selected now looks like this:";
            foreach (WeightEntry e in _selected) selectedList = $"{selectedList} / #{e.Id}";
            Log(selectedList);
        }

        public void DeleteSelected()
        {
            if (Services == null) throw new Exception("Null Services");
            if (_selected.Count == 0) return;
            string deletionList = $"Delete selected entries ({_selected.Count})?:\n";
            foreach (WeightEntry entry in _selected.ToList()) deletionList += $"\n - #{entry.Id} {entry.Time.ToString(TimeFormat)} \"{entry.Weight}{UnitLabel}\"";
            MessageBoxResult response = MessageBox.Show(deletionList, "Delete Entries", MessageBoxButton.OKCancel);
            if (response == MessageBoxResult.OK)
            {
                foreach (WeightEntry entry in _selected.ToList()) Services.weightLogService.DeleteEntry(entry.Id);
                Populate();
                DrawGraph();
            }
        }

        public void DrawGraph()
        {
            if (Services == null) throw new Exception("Null Services");
            int dayMargin = (int)Math.Ceiling(Services.SettingsService.Settings.WeightGraphLength / 2.0);
            if (Period is null)
            {
                Log("Warning: Null Period, returning.", Core.Logging.LogLevel.Warning);
                return;
            }
            GraphStartTime = Period.StartTime.AddDays(-dayMargin);
            GraphEndTime = Period.StartTime.AddDays(dayMargin);
            List<WeightEntry> weightLog = Services.dataService.GetWeightEntries(GraphStartTime.AddDays(-5), GraphEndTime.AddDays(5)); // additional 5 days to have data before & after.

            // Convert if needs be:
            if (Format != WeightFormat.Kg)
            {
                foreach (WeightEntry entry in weightLog) entry.Weight = WeightFormat.Kg.ConvertTo(Format, entry.Weight);
            }
            // I'm not testing that, it might work, it might not, I don't care.

            IReadOnlyList<DataPoint> weightDataPoints = ConvertToDataPoints(weightLog);
            PlotSeries WeightSeries = new()
            {
                SeriesType = SeriesType.LineContinuous,
                DataPoints = weightDataPoints,
                SeriesColor = SeriesColor.LineSeriesBrush1,
                ShowTrendline = true,
                ShowTrendLineStdDev = true
            };
            List<DataPoint> Highlight = new();
            Highlight.Add(new DataPoint { Time = Period.StartTime, Value = 1 });
            Highlight.Add(new DataPoint { Time = Period.EndTime, Value = 1 });
            IReadOnlyList<DataPoint> HighlightDataPoints = Highlight;
            PlotSeries HighlightSeries = new()
            {
                SeriesType = SeriesType.Highlight,
                DataPoints = HighlightDataPoints,
                SeriesColor = SeriesColor.HighLight1
            };
            IReadOnlyList<PlotSeries> seriesSet = new List<PlotSeries>
            { 
                HighlightSeries,
                WeightSeries
            };
            GraphSeriesSet = seriesSet;
        }

        private IReadOnlyList<DataPoint> ConvertToDataPoints(List<WeightEntry> weightLog)
        {
            // Copied this from input. Really should these defined more generally than making a new one every time but we're at that stage of the project I suppose...
            if (Services == null) throw new Exception("Null Services");
            List<DataPoint> dataPoints = new();
            foreach (WeightEntry entry in weightLog) dataPoints.Add(new DataPoint { Time = entry.Time, Value = entry.Weight });
            dataPoints = dataPoints.OrderBy(w => w.Time).ToList();
            return dataPoints;
        }

        
    }

    // We're making record types and such just inside of VM (files), we really are at that stage of the project!
    public sealed record SelectionChanged(WeightEntry entry, bool IsSelected);
}
