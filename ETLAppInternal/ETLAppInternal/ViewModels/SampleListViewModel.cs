using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ETLAppInternal.Constants;
using ETLAppInternal.Contracts.Services.Data;
using ETLAppInternal.Contracts.Services.General;
using ETLAppInternal.Extensions;
using ETLAppInternal.Models.Materials;
using ETLAppInternal.Models.Samples;
using ETLAppInternal.Models.Sql;
using ETLAppInternal.ViewModels.Base;
using Xamarin.Forms;

namespace ETLAppInternal.ViewModels
{
    public class SampleListViewModel : ViewModelBase
    {
        private readonly ISqlLiteService _dataService;
        private readonly ISettingsService _settingsService;
        #region properties
        private ObservableCollection<Samples> _filteredSamples;
        private ObservableCollection<Samples> _samples;
        private Materials Material;
        private string _search;
        public string _header;

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

        public ObservableCollection<Samples> Samples
        {
            get => _samples;
            set
            {
                _samples = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Samples> FilteredSamples
        {
            get => _filteredSamples;
            set
            {
                _filteredSamples = value;
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

        public string Header
        {
            get => _header;
            set
            {
                _header = value;
                OnPropertyChanged();
            }
        }
        #endregion
        
        private void FilterChanged(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                FilteredSamples = Samples;
            }
            else
            {
                FilteredSamples
                    = Samples.Where(x => x.ClientSampleId.Contains(value)
                                           ||
                                           (!string.IsNullOrEmpty(x.SampleLocation)
                                            && x.SampleLocation.ToLower().Contains(value.ToLower()))).ToObservableCollection();
            }

            FilteredSamples = FilteredSamples.Where(x => !x.Delete).ToObservableCollection();
        }
        
        public SampleListViewModel(IConnectionService connectionService, 
            INavigationService navigationService, 
            IDialogService dialogService,
            ISqlLiteService dataService, ISettingsService settingsService) 
            : base(connectionService, navigationService, dialogService)
        {
            _dataService = dataService;
            _settingsService = settingsService;
            InitializeMessenger();
        }
        public ICommand DoneCommand => new Command(async () => { await _navigationService.NavigateBackAsync(); });
        public ICommand SampleTappedCommand => new Command<Samples>(OnSampleTapped);
        public ICommand AddCommand => new Command(AddSample);
        public ICommand DeleteCommand => new Command(async (data) =>
        {
            await DeleteSample((Samples)data);
            await LoadSamples();
        });

        private async Task DeleteSample(Samples data)
        {
            if (data == null) return;
            var maxId = int.Parse(_settingsService.GetItem(Constants.DatabaseConstants.LastSampleId));
            if (data.Id <= maxId)
            {
                await _dialogService.ShowDialog(
                    "You can't delete a sample that is already on the server.",
                    "Error",
                    "OK");
                return;
            }
            var result = await _dialogService.ShowConfirm(
                "Are you sure you want to delete this sample?",
                "Delete Sample",
                "Yes", "Cancel");
            if (!result) return;
            await _dataService.DeleteAsync<SamplesTable>(data.ToSamplesTable().Id);
        }

        private async void AddSample()
        {
            var sampleId = await _dataService.GetLastSampleIdAsync();
            var newSample = new Samples
            {
                JobId = Material.JobId,
                MaterialId = Material.Id,
                IsNew = true,
                IsLocal = true,
                DateCollected = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff"),
                SampleDescription = Material.Description,
                Id = sampleId + 1
            };
            var id = 'a';
            for (var i = 0; i < Samples.Count; i++)
            {
                id++;
            }
            newSample.ClientSampleId = string.Concat(Material.ClientMaterialId, id.ToString());
            await _navigationService.NavigateToAsync<SampleDetailViewModel>(newSample);
        }

        private async void OnSampleTapped(Samples data)
        {
            if (data == null) return;
            await _navigationService.NavigateToAsync<SampleDetailViewModel>(data);
        }

        private void InitializeMessenger()
        {
            MessagingCenter.Subscribe<SampleDetailViewModel>(this,
                MessengerConstants.SampleDetailViewModel_SampleAdded, async (sender) =>
                {
                    await LoadSamples();
                });
        }


        #region load
        private async Task LoadSamples()
        {
            _search = string.Empty;
            _samples = (await _dataService.GetSamplesAsync(Material.Id)).ToObservableCollection();
            FilteredSamples = _samples.Where(x => !x.Delete).ToObservableCollection();
        }
        public override async Task InitializeAsync(object data)
        {
            ShowMenu = false;
            IsBusy = true;
            Material = (Materials)data;
            Header = string.Concat(Material.ClientMaterialId, " ", Material.Material);
            await LoadSamples();
            IsBusy = false;
        }
        #endregion
    }
}
