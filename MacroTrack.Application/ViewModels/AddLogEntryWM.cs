using MacroTrack.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.AppLibrary.ViewModels
{
    public class AddLogEntryWM : ViewModelBase
    {      

        public AddLogEntryWM()
        {
        }

        private string _lastAdded = "Last added: none";
        public string LastAdded
        {
            get => _lastAdded;
            set { _lastAdded = value; OnPropertyChanged(); }
        }
    }
}
