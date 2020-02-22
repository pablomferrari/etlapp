using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ETLAppInternal.Contracts.Services.Data;
using ETLAppInternal.Contracts.Services.General;
using ETLAppInternal.Extensions;
using ETLAppInternal.ViewModels.Base;
using Xamarin.Forms;

namespace ETLAppInternal.ViewModels
{
    public class HistoryListViewModel : ViewModelBase
    {
        private ObservableCollection<string> _deliveries;
        public ObservableCollection<string> Deliveries
        {
            get => _deliveries;
            set
            {
                _deliveries = value;
                OnPropertyChanged();
            }
        }
        private readonly IDataService _cacheService;
        public HistoryListViewModel(
            IConnectionService connectionService, 
            INavigationService navigationService, 
            IDialogService dialogService, IDataService cacheService) : base(connectionService, navigationService, dialogService)
        {
            _cacheService = cacheService;
        }

        public ICommand SelectedCommand => new Command(async (data) =>
        {
            await _navigationService.NavigateToAsync<HistoryDetailViewModel>(data);
        });

        public override async Task InitializeAsync(object data)
        {
            await Task.Run(() =>
            {
                IsBusy = true;
                Deliveries = _cacheService.GetKeys().ToObservableCollection();
                IsBusy = false;
            });
        }
    }
}
