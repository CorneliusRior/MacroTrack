using MacroTrack.AppLibrary.Controls;
using MacroTrack.AppLibrary.Graphs;
using MacroTrack.AppLibrary.Services;
using MacroTrack.Core.DataModels;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Services;
using MacroTrack.Core.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MacroTrack.AppLibrary.ViewModels
{
    public class SummaryVM : ViewModelBase
    {
        private MacroSummary? _currentSummary;
        public MacroSummary? CurrentSummary
        {
            get => _currentSummary;
            set
            {
                if (_currentSummary == value) return;
                _currentSummary = value;
                OnPropertyChanged();
                Populate();
            }
        }

        private string _calPct = "(-%)";
        public string CalPct
        {
            get => _calPct;
            set
            {
                if (_calPct == value) return;
                _calPct = value;
                OnPropertyChanged();
            }
        }

        private string _proPct = "(-%)";
        public string ProPct
        {
            get => _proPct;
            set
            {
                if (_proPct == value) return;
                _proPct = value; 
                OnPropertyChanged();
            }
        }

        private string _carPct = "(-%)";
        public string CarPct
        {
            get => _carPct;
            set
            {
                if (_carPct == value) return;
                _carPct = value;
                OnPropertyChanged();
            }
        }

        private string _fatPct = "(-%)";
        public string FatPct
        {
            get => _fatPct;
            set
            {
                if (_fatPct == value) return;
                _fatPct = value;
                OnPropertyChanged();
            }
        }

        private MacroSingleType _calSingle = new MacroSingleType(MacroType.Calories, 0, 0, 0, null, null);
        public MacroSingleType CalSingle
        {
            get => _calSingle;
            set
            {
                if (_calSingle == value) return;
                _calSingle = value;
                OnPropertyChanged();
            }
        }

        private MacroSingleType _proSingle = new MacroSingleType(MacroType.Protein, 0, 0, 0, null, null);
        public MacroSingleType ProSingle
        {
            get => _proSingle;
            set
            {
                if (_proSingle == value) return;
                _proSingle = value;
                OnPropertyChanged();
            }
        }

        private MacroSingleType _carSingle = new MacroSingleType(MacroType.Carbs, 0, 0, 0, null, null);
        public MacroSingleType CarSingle
        {
            get => _carSingle;
            set
            {
                if (_carSingle == value) return;
                _carSingle = value;
                OnPropertyChanged();
            }
        }

        private MacroSingleType _fatSingle = new MacroSingleType(MacroType.Fat, 0, 0, 0, null, null);
        public MacroSingleType FatSingle
        {
            get => _fatSingle;
            set
            {
                if (_fatSingle == value) return;
                _fatSingle = value;
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

        private TimePeriod? _previousPeriod;
        public TimePeriod? PreviousPeriod
        {
            get => _previousPeriod;
            set
            {
                if (_previousPeriod == value) return;
                _previousPeriod = value;
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

        // Functions:
        public override void Init(CoreServices services, AppServices appServices)
        {
            base.Init(services, appServices);
            AppServices!.AppEvents.Subscribe<FoodLogChanged>(_ => DrawGraph()); // Just draws graph as the rest updates automatically.
            AppServices!.AppEvents.Subscribe<SettingsChanged>(_ =>
            {
                GraphSettings = Services!.SettingsService.Settings.GraphSettings;
                DrawGraph();
            });
            AppServices.AppEvents.Subscribe<SummaryChanged>(_ => DrawGraph());
            GraphSettings = Services!.SettingsService.Settings.GraphSettings;                        
            Populate();
            DrawGraph();
            
        }

        public void Populate()
        {
            if (CurrentSummary == null)
            {
                CalSingle = new MacroSingleType(MacroType.Calories, 0, 0, 0, null, null);
                CalPct = "(-%)";
                ProPct = "(-%)";
                CarPct = "(-%)";
                FatPct = "(-%)";
            }
            else
            {
                CalSingle = CurrentSummary.GetSingle(MacroType.Calories);
                ProSingle = CurrentSummary.GetSingle(MacroType.Protein);
                CarSingle = CurrentSummary.GetSingle(MacroType.Carbs);
                FatSingle = CurrentSummary.GetSingle(MacroType.Fat);
                CalPct = FormatPct(CurrentSummary.Actual.Calories, CurrentSummary.Target.Calories);
                ProPct = FormatPct(CurrentSummary.Actual.Protein, CurrentSummary.Target.Protein);
                CarPct = FormatPct(CurrentSummary.Actual.Carbs, CurrentSummary.Target.Carbs);
                FatPct = FormatPct(CurrentSummary.Actual.Fat, CurrentSummary.Target.Fat);
            }
        }

        public void DrawGraph()
        {
            if (Services == null) throw new Exception("Null Services");
            bool excludeCheatDays = true; // make this a Settings Property
            // Get data:
            if (PreviousPeriod is not null)
            {
                GraphStartTime = PreviousPeriod.StartTime.AddDays(-15);
                GraphEndTime = PreviousPeriod.EndTime.AddDays(15);
            }
            else
            {
                GraphStartTime = DateTime.Today.AddDays(-Services.SettingsService.Settings.CalGraphLength);
                GraphEndTime = DateTime.Today.AddDays(1);
            }
            
            List<(DateTime date, double value)> actualCalList = Services.foodLogService.DailySumRange("Calories", GraphStartTime.AddDays(-1), GraphEndTime.AddDays(1));
            List<(DateTime date, double value)> goalCalList = Services.goalService.GetTupleGoalHistory(GraphStartTime.AddDays(-1), GraphEndTime.AddDays(1), true);
            List<(DateTime date, double value)> cheatDays = Services.taskService.GetCheatDayTupleRange(GraphStartTime.AddDays(-1), GraphEndTime.AddDays(1));
            // When you get back apply cheat by adding "excluded dates" to PlotSeries.

            // Generate PlotSeries:
            PlotSeries ActualSeries = new()
            {
                SeriesType = SeriesType.LineDiscreteDaily,
                DataPoints = TupleToDataPoints(actualCalList),
                SeriesColor = SeriesColor.LineSeriesBrush1,
                ExcludedPoints = excludeCheatDays ? TupleToDataPoints(cheatDays) : null
            };
            PlotSeries GoalSeries = new()
            {
                SeriesType = SeriesType.StepLine,
                DataPoints = TupleToDataPoints(goalCalList),
                SeriesColor = SeriesColor.LineSeriesBrush2
            };
            PlotSeries CheatSeries = new()
            {
                SeriesType = SeriesType.DaysBinary,
                DataPoints = TupleToDataPoints(cheatDays),
                SeriesColor = SeriesColor.DayBinarySeriesBrush1,
                Enabled = excludeCheatDays
            };

            // Make Highlight in case it is needed:
            PlotSeries CurrentPeriodHighlight = new()
            {
                SeriesType = SeriesType.Highlight,
                DataPoints = TimePeriodToDataPoints(PreviousPeriod),
                SeriesColor = SeriesColor.HighLight1
            };

            // Add to SeriesSet:
            if (PreviousPeriod is null)
            {
                IReadOnlyList<PlotSeries> seriesSet = new List<PlotSeries>
                {
                    ActualSeries,
                    GoalSeries,
                    CheatSeries
                };
                GraphSeriesSet = seriesSet;
            }
            else
            {
                IReadOnlyList<PlotSeries> seriesSet = new List<PlotSeries>
                {
                    CurrentPeriodHighlight,
                    ActualSeries,
                    GoalSeries,
                    CheatSeries
                };
                GraphSeriesSet = seriesSet;
            }
            
            LogVars(new { GraphSeriesSet }, "This is the GraphSeriesSet rn: ");
        }

        private IReadOnlyList<DataPoint> TupleToDataPoints(List<(DateTime time, double value)> tupleList)
        {
            if (Services == null) throw new Exception("Null Services");
            List<DataPoint> dataPoints = new();
            foreach (var t in tupleList) dataPoints.Add(new DataPoint { Time = t.time, Value = t.value });
            dataPoints = dataPoints.OrderBy(p => p.Time).ToList();
            
            return dataPoints;
        }

        private IReadOnlyList<DataPoint> TimePeriodToDataPoints(TimePeriod? period)
        {
            if (Services == null) throw new Exception("Null Services");
            List<DataPoint> dataPoints = new();
            if (period is null) return dataPoints;
            dataPoints.Add(new DataPoint { Time = period.StartTime, Value = 1 });
            dataPoints.Add(new DataPoint { Time = period.EndTime, Value = 1 });
            return dataPoints;
        }

        private string FormatPct(double? actual, double? target)
        {
            if (actual == null || target == null || target == 0)
            {
                return "(-%)";
            }
            return $"({((double)((actual / target) * 100)).ToString("0.0")}%)";
        }

              
    }
}
