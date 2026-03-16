using MacroTrack.AppLibrary.Commands;
using MacroTrack.AppLibrary.Services;
using MacroTrack.Core.Models;
using MacroTrack.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MacroTrack.AppLibrary.ViewModels
{
    public class PresetManageVM : ViewModelBase
    {
        public event Action<bool>? RequestClose;
        public ObservableCollection<Preset> PresetList { get; } = new();
        public ObservableCollection<string> CatList { get; } = new();

        private Preset? _selectedPreset;
        public Preset? SelectedPreset
        {
            get => _selectedPreset;
            set
            {
                p();
                if (_selectedPreset == value) return;
                _selectedPreset = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasSelected));
                UpdateSelected();
            }
        }

        private Preset? _selectedEditable;
        public Preset? SelectedEditable
        {
            get => _selectedEditable;
            set
            {
                p();
                if (_selectedEditable == value) return;
                _selectedEditable = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasSelected));
            }
        }

        private bool _selectedStoreWeight;
        public bool SelectedStoreWeight
        {
            get => _selectedStoreWeight;
            set
            {
                p($"Updating from {_selectedStoreWeight} to {value}");
                if (_selectedStoreWeight == value) return;
                _selectedStoreWeight = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedUnitGram));
                OnPropertyChanged(nameof(SelectedUnitMl));
            }
        }

        private double _selectedWeight;
        public double SelectedWeight
        {
            get => _selectedWeight;
            set
            {
                p($"Updating from {_selectedWeight} to {value}");
                if (_selectedWeight == value) return;
                _selectedWeight = value;
                OnPropertyChanged();
            }
        }

        private bool _selectedUnitGram;
        public bool SelectedUnitGram
        {
            get => _selectedUnitGram;
            set
            {
                p($"Updating from {_selectedUnitGram} to {value}");
                if (_selectedUnitGram == value) return;
                _selectedUnitGram = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedUnitMl));
            }
        }

        private bool _selectedUnitMl;
        public bool SelectedUnitMl
        {
            get => _selectedUnitMl;
            set
            {
                p($"Updating from {_selectedUnitMl} to {value}");
                if (_selectedUnitMl == value) return;
                _selectedUnitMl = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedUnitGram));
            }
        }

        private bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                p($"IsEditing updating from {_isEditing} to {value}");
                if (_isEditing == value) return;
                _isEditing = value;
                OnPropertyChanged();
            }
        }

        public bool HasSelected => SelectedPreset is not null;


        // Commands:
        public ICommand CloseCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand OpenEditCommand { get; }
        public ICommand OpenViewCommand { get; }
        public PresetManageVM()
        {
            CloseCommand = new RelayCommand(() => RequestClose?.Invoke(false));
            EditCommand = new RelayCommand(() => Edit());
            OpenEditCommand = new RelayCommand(() => OpenEdit());
            OpenViewCommand = new RelayCommand(() => OpenView());
        }

        public override void Init(CoreServices services, AppServices appServices)
        {
            base.Init(services, appServices);
            EventSubscribe(AppServices!.AppEvents.Subscribe<GeneralRefresh>(_ => Populate(SelectedPreset?.Id)));
            EventSubscribe(AppServices!.AppEvents.Subscribe<PresetListChanged>(_ => Populate(SelectedPreset?.Id)));
            Populate();
        }

        private void Populate(int? id = null)
        {
            if (Services is null) throw new Exception("Null Services");
            List<string> catList = Services.presetService.GetCategoryList();
            CatList.Clear();
            foreach (string c in catList) CatList.Add(c);
            List<Preset> presetList = Services.presetService.GetAll();
            PresetList.Clear();
            foreach (Preset p in presetList) PresetList.Add(p);
            if (id is not null) SelectedPreset = PresetList.FirstOrDefault(p => p.Id == id);
        }

        private void Edit()
        {
            if (SelectedEditable is null) return;
            if (Services is null) throw new Exception("Null Services");
            Services.presetService.EditEntry(SelectedEditable.Id, SelectedEditable.PresetName, SelectedEditable.Calories, SelectedEditable.Protein, SelectedEditable.Carbs, SelectedEditable.Fat, SelectedStoreWeight ? SelectedWeight : null, SelectedStoreWeight ? SelectedUnitGram ? "g" : "ml" : null, SelectedEditable.Category, SelectedEditable.Notes);
            Populate(SelectedEditable.Id);
            OpenView();
            if (AppServices is not null) AppServices.AppEvents.Publish(new PresetListChanged());
        }

        private void OpenEdit()
        {
            p("In edit");
            if (SelectedPreset is null) return;
            SelectedEditable = SelectedPreset.Clone();
            p("Affirming we get to isEditing");
            IsEditing = true;
        }

        private void OpenView()
        {
            SelectedEditable = null;
            IsEditing = false;
        }

        private void UpdateSelected()
        {
            if (SelectedPreset is null) return;
            if (SelectedPreset.Weight is not null && SelectedPreset.Unit is not null)
            {
                p("Neither null");
                SelectedStoreWeight = true;
                SelectedWeight = SelectedPreset.Weight.Value;
                p($"SelectedPreset.Weight={SelectedPreset.Weight}, SelectedWeight={SelectedWeight}");
                SelectedUnitGram = SelectedPreset.Unit == "g";
                SelectedUnitMl = SelectedPreset.Unit == "ml";
            }
            else
            {
                p("One or both null");
                SelectedStoreWeight = false;
                SelectedWeight = 100.0;
                SelectedUnitGram = true;
                SelectedUnitMl = false;
            }
            if (IsEditing) SelectedEditable = SelectedPreset.Clone();
        }
    }
}
