using System.Threading.Tasks;
using ETLAppInternal.Contracts.Services.General;
using ETLAppInternal.ViewModels.Base;

namespace ETLAppInternal.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private MenuViewModel _menuViewModel;

        public MainViewModel(IConnectionService connectionService,
            INavigationService navigationService, IDialogService dialogService,
            MenuViewModel menuViewModel)
            : base(connectionService, navigationService, dialogService)
        {
            _menuViewModel = menuViewModel;
        }

        public override async Task InitializeAsync(object data)
        {
            await Task.WhenAll
            (
                _menuViewModel.InitializeAsync(data),
                _navigationService.NavigateToAsync<JobListViewModel>()
            );
        }
    }
}
