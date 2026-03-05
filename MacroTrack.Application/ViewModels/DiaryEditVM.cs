using MacroTrack.AppLibrary.Services;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Models;
using MacroTrack.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MacroTrack.AppLibrary.ViewModels
{
    public class DiaryEditVM : ViewModelBase
    {
        public event Action<bool>? RequestClose;

        private string _headerText = "No Entry";
        public string HeaderText
        {
            get => _headerText;
            set
            {
                if (_headerText == value) return;
                _headerText = value;
            }
        }

        private string _entryBody = "";
        public string EntryBody
        {
            get => _entryBody;
            set
            {
                if (_entryBody == value) return;
                _entryBody = value;
            }
        }

        private string _editNotes = "";
        public string EditNotes
        {
            get => _editNotes;
            set
            {
                if (_editNotes == value) return;
                _editNotes = value;
            }
        }

        public override void Init(CoreServices services, AppServices appServices)
        {
            base.Init(services, appServices);
        }

        public void Populate(DiaryEntry entry)
        {
            if (Services == null) return;
            string TimeFormat = Services.SettingsService.GetDateTimeString(
                Services.SettingsService.Settings.DiaryTimeIsLongFormat,
                Services.SettingsService.Settings.DiaryTimeIncludesTime,
                Services.SettingsService.Settings.DiaryTimeIncludesSeconds
            );

            HeaderText = $"Entry #{entry.Id}: {entry.Time.ToString(TimeFormat)}";
            EntryBody = entry.Body;
        }

        /// <summary>
        /// This is called with the assumption that you already clicked yes in the messagebox or whatever.
        /// </summary>
        public void Edit(DiaryEntry entry)
        {
            try
            {
                if ( Services == null) throw new Exception("Null Services");
                DiaryEntry AddedEntry = Services!.diaryService.EditEntry(entry.Id, EntryBody, EditNotes);
                if (AddedEntry == null) throw new Exception("Entry returned as null, uncaught error?");
                AppServices?.AppEvents.Publish(new DiaryChanged());
                RequestClose?.Invoke(true);
            }
            catch (Exception ex) 
            { 
                Log("Error editing Diary entry, returning.", LogLevel.Warning, ex);
                MessageBox.Show("Error editing diary entry, please check logs.");
            }            
        }

        public void Delete(DiaryEntry entry)
        {
            try
            {
                if (Services == null) throw new Exception("Null Services");
                DiaryEntry DeletedEntry = Services.diaryService.DeleteEntry(entry.Id);
                if (DeletedEntry== null) throw new Exception("Entry returned as null, uncaught error?");
                AppServices?.AppEvents.Publish(new DiaryChanged());
                RequestClose?.Invoke(true);
            }
            catch (Exception ex) 
            { 
                Log("Error Deleting Diary entry.", LogLevel.Warning, ex); 
                MessageBox.Show("Error deleting diary entry, please check logs.");
            }
        }

        
    }
}
