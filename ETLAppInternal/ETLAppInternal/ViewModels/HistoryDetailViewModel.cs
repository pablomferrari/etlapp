using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ETLAppInternal.Contracts.Services.Data;
using ETLAppInternal.Contracts.Services.General;
using ETLAppInternal.Extensions;
using ETLAppInternal.Models.Materials;
using ETLAppInternal.Models.Samples;
using ETLAppInternal.ViewModels.Base;
using Xamarin.Forms;

namespace ETLAppInternal.ViewModels
{
    public class HistoryDetailViewModel : ViewModelBase
    {
        private readonly IDataService _cacheService;
        private readonly IApiService _apiService;
        public HistoryDetailViewModel(IConnectionService connectionService, INavigationService navigationService,
            IDialogService dialogService, IDataService cacheService, IApiService apiService) : base(connectionService, navigationService, dialogService)
        {
            _cacheService = cacheService;
            _apiService = apiService;
        }

        private ObservableCollection<Materials> _newMaterials;

        public ObservableCollection<Materials> NewMaterials
        {
            get => _newMaterials;
            set
            {
                _newMaterials = value; OnPropertyChanged();
            }
        }

        private ObservableCollection<Materials> _updatedMaterials;

        public ObservableCollection<Materials> UpdatedMaterials
        {
            get => _updatedMaterials;
            set
            {
                _updatedMaterials = value; OnPropertyChanged();
            }
        }

        private ObservableCollection<Samples> _updatedSamples;
        public ObservableCollection<Samples> UpdatedSamples
        {
            get => _updatedSamples;
            set
            {
                _updatedSamples = value; OnPropertyChanged();
            }
        }

        private ObservableCollection<Samples> _newSamples;
        public ObservableCollection<Samples> NewSamples
        {
            get => _newSamples;
            set
            {
                _newSamples = value; OnPropertyChanged();
            }
        }

        private string _originalKey { get; set; }
        private string _key;

        public string Key
        {
            get => _key;
            set { _key = value; OnPropertyChanged(); }
        }
        public override async Task InitializeAsync(object data)
        {
            await Task.Run(async () =>
            {
                IsBusy = true;
                _originalKey = (string) data;
                Key = _originalKey.Replace("_", " ");
                var syncedData = await _cacheService.GetSurveyDataAsync(_originalKey);
                NewMaterials = syncedData.NewMaterials.ToObservableCollection();
                UpdatedMaterials = syncedData.UpdatedMaterials.ToObservableCollection();
                var newSamples = NewMaterials.SelectMany(x => x.Samples).ToList();
                newSamples.AddRange(syncedData.NewSamples.ToList());
                NewSamples = newSamples.ToObservableCollection();
                UpdatedSamples = syncedData.UpdatedSamples.ToObservableCollection();
                IsBusy = false;
            });
        }


        public ICommand ResendCommand => new Command(async () =>
        {
            var isReachable = await _connectionService.IsApiReachable();
            if (!isReachable)
            {
                await _dialogService.ShowDialog(
                    "Either you are not connected to internet or server is down at this moment. Please try again later.",
                    "Error",
                    "OK");
                return;
            }

            try
            {
                var data = await _cacheService.GetSurveyDataAsync(_originalKey);
                try
                {
                    IsBusy = true;
                    await _apiService.PostData(data);
                    IsBusy = false;
                    await _navigationService.NavigateBackAsync();
                }
                catch (Exception e)
                {
                    _dialogService.ShowToast("Error posting data: " + e.Message);
                    IsBusy = false;
                }
            }
            catch (Exception)
            {
                IsBusy = false;
                _dialogService.ShowToast("Data is removed after 15 days. This data is unavailable.");
                await _navigationService.NavigateBackAsync();
            }
           
            
           
            
        });
    }
}
