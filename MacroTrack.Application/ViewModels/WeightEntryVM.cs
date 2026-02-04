using MacroTrack.Core.Logging;
using MacroTrack.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.AppLibrary.ViewModels
{
    internal class WeightEntryVM : ViewModelBase
    {
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

        private double? _weight;
        public double? Weight
        {
            get => _weight;
            set
            {
                Log();
                if (_weight == value) return;
                _weight = value;
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
            Weight = null;
            ClearAllErrors();
        }

        public void ClearAllErrors()
        {
            ClearError(nameof(Time));
            ClearError(nameof(Weight));
        }

        public void Add()
        {
            Log();
            bool ok = true;
            ok &= DateTimeRequire(nameof(Time), Time);
            ok &= NumericRequire(nameof(Weight), Weight);
            if (!ok) return;

            DateTime time = Time!.Value;
            double weight = Weight!.Value;

            try
            {
                if (Services == null) throw new Exception("Null Services");
                WeightEntry entry = Services.weightLogService.AddEntry(time, weight);
                Log($"Added entry #{entry.Id}", LogLevel.Info);
                Clear();
            }
            catch (Exception ex) { Log("Could not add entry", LogLevel.Error, ex); }
        }
    }
}
