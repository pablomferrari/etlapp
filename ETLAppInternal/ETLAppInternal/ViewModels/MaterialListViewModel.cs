using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ETLAppInternal.Constants;
using ETLAppInternal.Contracts.Services.Data;
using ETLAppInternal.Contracts.Services.General;
using ETLAppInternal.Extensions;
using ETLAppInternal.Models.Jobs;
using ETLAppInternal.Models.Materials;
using ETLAppInternal.Models.Sql;
using ETLAppInternal.ViewModels.Base;
using Xamarin.Forms;

namespace ETLAppInternal.ViewModels
{
    public class MaterialListViewModel : ViewModelBase
    {
        private readonly ISqlLiteService _dataService;
        private readonly ISettingsService _settingsService;
        #region props
        private bool _showMenu;

        public bool ShowMenu
        {
            get => _showMenu;
            set
            {
                _showMenu = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Materials> _filteredMaterials;
        private ObservableCollection<Materials> _materials;
        public ObservableCollection<Materials> Materials
        {
            get => _materials;
            set
            {
                _materials = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Materials> FilteredMaterials
        {
            get => _filteredMaterials;
            set
            {
                _filteredMaterials = value;
                OnPropertyChanged();
            }
        }
        private Jobs Job;
        private string _search;
        public string _header;
        public string _address;
        public string _status;
        public string Header
        {
            get => _header;
            set
            {
                _header = value;
                OnPropertyChanged();
            }
        }

        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged();
            }
        }

        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        public string Search
        {
            get => _search;
            set
            {
                _search = value;
                FilterChanged(value);
                OnPropertyChanged();
            }
        }
        #endregion
        public MaterialListViewModel(IConnectionService connectionService,
            INavigationService navigationService,
            IDialogService dialogService,
            ISqlLiteService dataService,
            ISettingsService settings)
            : base(connectionService, navigationService, dialogService)
        {
            _dataService = dataService;
            _settingsService = settings;
            InitializeMessenger();
        }

        public void InitializeMessenger()
        {
            MessagingCenter.Subscribe<MaterialDetailViewModel>(this,
                MessengerConstants.MaterialDetailViewModel_MaterialAdded,
                async (sender) => await LoadMaterials());
        }

        public ICommand MaterialTappedCommand => new Command<Materials>(OnMaterialTapped);
        public ICommand AddCommand => new Command(AddMaterialCommand);

        public ICommand DeleteCommand => new Command(async (data) =>
        {
            await DeleteMaterial((Materials)data);
            await LoadMaterials();
        });

        public ICommand ViewSamplesCommand => new Command<Materials>(ViewSamples);

        public ICommand DoneCommand => new Command(async () => { await _navigationService.NavigateBackAsync(); });

        private async void ViewSamples(Materials data)
        {
            await _navigationService.NavigateToAsync<SampleListViewModel>(data);
        }


        private async Task DeleteMaterial(Materials data)
        {
            if (data == null) return;
            var matIdStr = _settingsService.GetItem(Constants.DatabaseConstants.LastMaterialId);
            var matId =  int.Parse(matIdStr);
            if (data.Id <= matId)
            {
                await _dialogService.ShowDialog(
                    "You can't delete a material that is already on the server.",
                    "Error",
                    "OK");
                return;
            }
            var result = await _dialogService.ShowConfirm(
                "Are you sure you want to delete this material?",
                "Delete Material",
                "Yes", "Cancel");
            if (!result) return;
            await _dataService.DeleteAsync<MaterialsTable>(data.ToMaterialTable().Id);
            var samples = await _dataService.GetSamplesAsync(data.Id);
            foreach (var sample in samples)
            {
                await _dataService.DeleteAsync<SamplesTable>(sample.Id);
            }
        }

        public ICommand DeliverCommand => new Command(DeliverSamples);

        private async void DeliverSamples(object obj)
        {
            await _navigationService.NavigateToAsync<DeliveriesViewModel>(Job.JobId);
        }

        private async void AddMaterialCommand()
        {
            var id = Materials.Count + 1;
            var newId = await _dataService.GetLastMaterialIdAsync();

            var newMaterial = new Materials
            {
                Id = newId + 1,
                JobId = Job.JobId,
                ClientMaterialId = id.ToString(),
                IsNew = true,
                Quantity = 0.0,
                CreatedBy = int.Parse(_settingsService.UserIdSetting),
                CreatedDate = DateTime.Now
            };
            await _navigationService.NavigateToAsync<MaterialDetailViewModel>(newMaterial);
        }

        private async void OnMaterialTapped(Materials data)
        {
            if (data == null) return;
            await _navigationService.NavigateToAsync<MaterialDetailViewModel>(data);
        }

        #region loading
        private void FilterChanged(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                FilteredMaterials = Materials.ToObservableCollection();
            }
            else
            {
                FilteredMaterials
                    = Materials.Where(x => x.ClientMaterialId.Contains(value)
                                                ||
                                                !string.IsNullOrEmpty(x.Material) 
                                                && x.Material.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0)
                        .ToObservableCollection();
            }
        }
        public override async Task InitializeAsync(object data)
        {
            IsBusy = true;
            Job = (Jobs)data;
            Header = string.Concat(Job.JobId, " ", Job.Client?.Name);
            Address = Job.FacilityAddress;
            Status = Job.Status?.Status;
            await LoadMaterials(false);
            IsBusy = false;
        }
        private async Task LoadMaterials(bool forceRefresh = false)
        {
            Materials = (await _dataService.GetMaterialsAsync(Job.JobId)).ToObservableCollection();
            FilteredMaterials = Materials ?? new ObservableCollection<Materials>();
            FilterChanged(_search);
        }
        #endregion
    }
}