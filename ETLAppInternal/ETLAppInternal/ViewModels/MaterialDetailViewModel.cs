using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ETLAppInternal.Constants;
using ETLAppInternal.Contracts.Services.Data;
using ETLAppInternal.Contracts.Services.General;
using ETLAppInternal.Extensions;
using ETLAppInternal.Models.Materials;
using ETLAppInternal.Models.Sql;
using ETLAppInternal.ViewModels.Base;
using Xamarin.Forms;

namespace ETLAppInternal.ViewModels
{
    public class MaterialDetailViewModel : ViewModelBase
    {
        public Materials CurrentMaterial;
        public bool _canAddSamples;
        public bool CanAddSamples
        {
            get => _canAddSamples;
            set
            {
                _canAddSamples = value;
                OnPropertyChanged();
            }
        }
        public IEnumerable<Mapping> Mappings;
        #region Properties
        #region Id
        public string _clientMaterialId;
        public string ClientMaterialId
        {
            get => _clientMaterialId;
            set
            {
                _clientMaterialId = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region Rooms
        public string _room;
        public string Room
        {
            get => _room;
            set
            {
                _room = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region Material
        public string _material;
        public string Material
        {
            get => _material;
            set
            {
                _material = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<string> _materials;
        public ObservableCollection<string> Materials
        {
            get => _materials;
            set
            {
                _materials = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region MaterialSub
        public string _materialSub;
        public string MaterialSub
        {
            get => _materialSub;
            set
            {
                _materialSub = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<string> _materialSubs;
        public ObservableCollection<string> MaterialSubs
        {
            get => _materialSubs;
            set
            {
                _materialSubs = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region Classifications

        private ObservableCollection<string> _classifications;
        public ObservableCollection<string> ClassificationList
        {
            get => _classifications;
            set
            {
                _classifications = value;
                OnPropertyChanged();
            }
        }

        public string Classification { private set; get; }

        private int _classificationIndex = -1;
        public int ClassificationIndex
        {
            get => _classificationIndex;
            set
            {
                if (value == _classificationIndex)
                    return;
                _classificationIndex = value;
                OnPropertyChanged(nameof(ClassificationIndex));
                if (_classificationIndex < _classifications.Count && _classificationIndex >= 0)
                {
                    Classification = _classifications[_classificationIndex];
                    OnPropertyChanged(nameof(Classification));
                }
            }
        }
        #endregion
        #region Units

        private ObservableCollection<string> _units;
        public ObservableCollection<string> UnitList
        {
            get => _units;
            set
            {
                _units = value;
                OnPropertyChanged();
            }
        }

        public string Unit { private set; get; }

        private int _unitIndex = -1;
        public int UnitIndex
        {
            get => _unitIndex;
            set
            {
                if (value == _unitIndex)
                    return;
                _unitIndex = value;
                OnPropertyChanged(nameof(UnitIndex));
                if (_unitIndex < _units.Count && _unitIndex >= 0)
                {
                    Unit = _units[_unitIndex];
                    OnPropertyChanged(nameof(Unit));
                }
            }
        }
        #endregion
        #region Friables

        private ObservableCollection<string> _friables;
        public ObservableCollection<string> FriableList
        {
            get => _friables;
            set
            {
                _friables = value;
                OnPropertyChanged();
            }
        }

        public string Friable { private set; get; }

        private int _friableIndex = -1;
        public int FriableIndex
        {
            get => _friableIndex;
            set
            {
                if (value == _friableIndex)
                    return;
                _friableIndex = value;
                OnPropertyChanged(nameof(FriableIndex));
                if (_friableIndex < _friables.Count && _friableIndex >= 0)
                {
                    Friable = _friables[_friableIndex];
                    OnPropertyChanged(nameof(Friable));
                }
            }
        }
        #endregion
        #region Color
        public string _color;
        public string Color
        {
            get => _color;
            set
            {
                _color = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<string> _colors;

        public ObservableCollection<string> Colors
        {
            get => _colors;
            set
            {
                _colors = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region Size
        public string _size;
        public string Size
        {
            get => _size;
            set
            {
                _size = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<string> _sizes;
        public ObservableCollection<string> Sizes
        {
            get => _sizes;
            set
            {
                _sizes = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region Assumed
        public bool _assumed;
        public bool Assumed
        {
            get => _assumed;
            set
            {
                _assumed = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region Quantity
        public string _quantityStr;
        public string QuantityStr
        {
            get => _quantityStr;
            set
            {
                _quantityStr = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region Note1
        public string _note1;
        public string Note1
        {
            get => _note1;
            set
            {
                _note1 = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region Note2
        public string _note2;
        public string Note2
        {
            get => _note2;
            set
            {
                _note2 = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #endregion

        private readonly ISqlLiteService _dataService;
        private readonly ISettingsService _settingsService;
        public MaterialDetailViewModel(
            IConnectionService connectionService,
            INavigationService navigationService,
            IDialogService dialogService,
            ISqlLiteService dataService, 
            ISettingsService settingsService)
            : base(connectionService, navigationService, dialogService)
        {
            _dataService = dataService;
            _settingsService = settingsService;
            InitializeMessenger();
        }

        private void InitializeMessenger()
        {
            MessagingCenter.Subscribe<SampleDetailViewModel>(this,
                MessengerConstants.SampleDetailViewModel_SampleAdded,
                (sender) =>
                {
                    UpdateMaterial();
                });

            MessagingCenter.Subscribe<RoomsViewModel, List<string>>(this,
                MessengerConstants.RoomsViewModel_RoomsAdded,
                (sender, val) =>
                {
                    RoomsAdded(val);
                });
        }

        private void RoomsAdded(List<string> val)
        {
            Room = string.Join(",", val);
        }

        private async void UpdateMaterial()
        {
            var updatedMaterial = BuildMaterial();
            await _dataService.InsertOrUpdateAsync(updatedMaterial.ToMaterialTable());
        }

        public ICommand ViewSamplesCommand => new Command(ViewSamples);
        public ICommand OnSaveChangesCommand => new Command(SaveMaterial);
        public ICommand MaterialChangedCommand => new Command(MaterialChanged);

        public ICommand AddRoomsCommand => new Command(AddRooms);

        private async void AddRooms()
        {
            await _navigationService.NavigateToAsync<RoomsViewModel>(Room);
        }

        private void MaterialChanged()
        {
            MapFields();
        }

        private void MapFields()
        {
            var result = Mappings.FirstOrDefault(x => x.Material == Material && x.MaterialSub == MaterialSub);
            if (result != null)
            {
                CurrentMaterial.Classification = result.Classification;
                CurrentMaterial.Friable = result.Friable;
                CurrentMaterial.Units = result.Units;
                ClassificationIndex = ClassificationList.IndexOf(CurrentMaterial.Classification);
                UnitIndex = UnitList.IndexOf(CurrentMaterial.Units);
                FriableIndex = FriableList.IndexOf(CurrentMaterial.Friable);
            }
        }

        public override async Task InitializeAsync(object data)
        {
            IsBusy = true;
            await LoadData();
            CurrentMaterial = (Materials)data;
            
            if (CurrentMaterial.IsNew)
            {
                CurrentMaterial.Units = UnitList?.FirstOrDefault();
                CurrentMaterial.Classification = ClassificationList?.FirstOrDefault();
                CanAddSamples = false;
            }
            else
            {
                CanAddSamples = true;
            }
            Material = CurrentMaterial.Material;
            MaterialSub = CurrentMaterial.MaterialSub;
            ClientMaterialId = CurrentMaterial.ClientMaterialId;
            Assumed = CurrentMaterial.Assumed;
            Classification = CurrentMaterial.Classification;
            Size = CurrentMaterial.Size;
            Unit = CurrentMaterial.Units;
            ClassificationIndex = ClassificationList?.IndexOf(CurrentMaterial.Classification) ?? 0;
            UnitIndex = UnitList?.IndexOf(CurrentMaterial.Units) ?? 0;
            FriableIndex = FriableList?.IndexOf(CurrentMaterial.Friable) ?? 0;
            Color = CurrentMaterial.Color;
            QuantityStr = CurrentMaterial.Quantity.ToString();
            Note1 = CurrentMaterial.Note1;
            Note2 = CurrentMaterial.Note2;
            Room = CurrentMaterial.Location;
            IsBusy = false;
        }

        private async Task LoadData()
        {

            Mappings = (await _dataService.GetMappingsAsync()).ToArray();
            
            var enumerable = Mappings as Mapping[];
            if (!enumerable.Any())
            {
                ClassificationList = new ObservableCollection<string>();
                Materials = new ObservableCollection<string>();
                MaterialSubs = new ObservableCollection<string>();
                FriableList = new ObservableCollection<string>();
                Colors = new ObservableCollection<string>();
                UnitList = new ObservableCollection<string>();
                Sizes = new ObservableCollection<string>();
                return;
            }
            _classifications = enumerable.Where(x => !string.IsNullOrEmpty(x.Classification))
                .Select(x => x.Classification).Distinct().ToObservableCollection();
            ClassificationList = _classifications ?? new ObservableCollection<string>();
            _materials = enumerable.Select(x => x.Material).Distinct().ToObservableCollection();
            Materials = _materials;
            _materialSubs = enumerable.Select(x => x.MaterialSub).Distinct().ToObservableCollection();
            MaterialSubs = _materialSubs;
            _friables = enumerable.Where(x => !string.IsNullOrEmpty(x.Friable))
                .Select(x => x.Friable).Distinct().ToObservableCollection();
            FriableList = _friables;
            _colors = MaterialHelper.Colors().ToObservableCollection();
            Colors = _colors;
            _units = enumerable.Select(x => x.Units).Distinct().ToObservableCollection();
            UnitList = _units;
            _sizes = MaterialHelper.Sizes().ToObservableCollection();
            Sizes = _sizes;

        }

        private Materials BuildMaterial()
        { 
            var material = new Materials
            {
                JobId = CurrentMaterial.JobId,
                Id = CurrentMaterial.Id,
                Location = Room,
                Material = Material,
                MaterialSub = MaterialSub,
                Color = Color,
                Quantity = double.Parse(QuantityStr),
                Assumed = Assumed,
                Classification = Classification,
                Friable = Friable,
                ClientMaterialId = ClientMaterialId,
                Size = Size,
                Units = Unit,
                Note1 = Note1,
                Note2 = Note2,
                IsLocal = CurrentMaterial.IsLocal,
                IsNew = CurrentMaterial.IsNew,
                CreatedBy = CurrentMaterial.CreatedBy,
                CreatedDate = CurrentMaterial.CreatedDate,
                EditedBy = CurrentMaterial.EditedBy,
                EditedDate = CurrentMaterial.EditedDate
            };
            return material;
        }

        private bool ValidateMaterial()
        {
            var missing = string.IsNullOrEmpty(CurrentMaterial.Material) ||
                         string.IsNullOrEmpty(CurrentMaterial.MaterialSub) ||
                         string.IsNullOrEmpty(CurrentMaterial.Classification) ||
                         string.IsNullOrEmpty(CurrentMaterial.Friable) ||
                         string.IsNullOrEmpty(CurrentMaterial.Size) ||
                         string.IsNullOrEmpty(CurrentMaterial.Units) ||
                         string.IsNullOrEmpty(CurrentMaterial.Color) ||
                         string.IsNullOrEmpty(CurrentMaterial.Location);
            return !missing;
        }
        private async void SaveMaterial()
        {
            CurrentMaterial = BuildMaterial();
            CurrentMaterial.IsLocal = true;
            CurrentMaterial.CanViewSamples = true;
            CurrentMaterial.EditedDate = DateTime.Now;
            CurrentMaterial.EditedBy = int.Parse(_settingsService.UserIdSetting);
            var result = ValidateMaterial();
            if (!result)
            {
                var continueSave = await _dialogService.ShowConfirm("Some fields are missing. Continue?", "Missing Fields",
                    "Yes", "No");
                if (!continueSave)
                {
                    return;
                }
            }

            var tableMaterial = CurrentMaterial.ToMaterialTable();
            await _dataService.InsertOrUpdateAsync<MaterialsTable>(tableMaterial);
            MessagingCenter.Send(this, MessengerConstants.MaterialDetailViewModel_MaterialAdded);
            MessagingCenter.Unsubscribe<SampleDetailViewModel>(this,
                MessengerConstants.SampleDetailViewModel_SampleAdded);
            MessagingCenter.Unsubscribe<RoomsViewModel, List<string>>(this,
                MessengerConstants.RoomsViewModel_RoomsAdded);

            await _navigationService.NavigateBackAsync();

            // ViewSamples();

        }


        private async void ViewSamples()
        {
            if (!CurrentMaterial.CanViewSamples && CurrentMaterial.Id <= 0)
            {
                await _dialogService.ShowDialog("Material needs to be saved first!", "Error", "OK");
            }
            else
            {
                await _navigationService.NavigateToAsync<SampleListViewModel>(CurrentMaterial);
            }
           
        }
    }
}
