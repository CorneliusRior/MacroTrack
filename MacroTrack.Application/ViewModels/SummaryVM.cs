using MacroTrack.AppLibrary.Controls;
using MacroTrack.AppLibrary.Services;
using MacroTrack.Core.DataModels;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
                Log("Should have changed", LogLevel.Info);
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

        public void Populate()
        {
            Log("We got here, print CurrentSummary things");
            LogVars(new { CurrentSummary, CurrentSummary?.GoalName, CurrentSummary?.Target.Calories }, "This is from VM");
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
                var a = CurrentSummary.GetSingle(MacroType.Calories);
                LogVars(new { a }, "Before");
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

        private string FormatPct(double? actual, double? target)
        {
            if (actual == null || target == null || target == 0)
            {
                return "(-%)";
            }
            return $"({((double)((actual / target) * 100)).ToString("0.0")}%)";
        }

        public override void Init(CoreServices services, AppServices appServices)
        {
            base.Init(services, appServices);
            Populate();
            Log("Reached Init");
            LogVars(new {CurrentSummary, CurrentSummary?.Actual.Calories});
        }        
    }
}
