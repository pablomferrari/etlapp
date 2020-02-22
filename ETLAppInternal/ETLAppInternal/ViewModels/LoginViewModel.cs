using System;
using System.Collections.Generic;
using System.Windows.Input;
using ETLAppInternal.Contracts.Services.Data;
using ETLAppInternal.Contracts.Services.General;
using ETLAppInternal.ViewModels.Base;
using Microsoft.AppCenter.Analytics;
using Xamarin.Forms;

namespace ETLAppInternal.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ISettingsService _settingsService;

        private string _userName;
        private string _password;

        public LoginViewModel(IConnectionService connectionService, ISettingsService settingsService,
            INavigationService navigationService,
            IAuthenticationService authenticationService,
            IDialogService dialogService)
            : base(connectionService, navigationService, dialogService)
        {
            _authenticationService = authenticationService;
            _settingsService = settingsService;
            NotBusy = !IsBusy;
        }

        public ICommand LoginCommand => new Command(OnLogin);

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        private async void OnLogin()
        {
            try
            {
                IsBusy = true;
                NotBusy = !IsBusy;                
                if (_connectionService.IsConnected)
                {
                    var authenticationResponse = await _authenticationService.Authenticate(UserName, Password);

                    if (authenticationResponse.IsAuthenticated)
                    {
                        // we store the Id to know if the user is already logged in to the application
                        _settingsService.UserIdSetting = authenticationResponse.Employee.Id.ToString();
                        _settingsService.UserNameSetting = authenticationResponse.Employee.Name;
                        Analytics.TrackEvent("Login", new Dictionary<string, string> {
                            { authenticationResponse.Employee.Name, DateTime.Now.ToString("G") }
                        });

                        IsBusy = false;
                        NotBusy = !IsBusy;
                        await _navigationService.NavigateToAsync<MainViewModel>();
                    }
                    else
                    {
                        await _dialogService.ShowDialog(
                            "This username/password combination isn't known",
                            "Error logging you in",
                            "OK");
                        IsBusy = false;
                        NotBusy = !IsBusy;
                    }
                }
                else
                {
                    _dialogService.ShowToast("You are not connected to internet!");
                    IsBusy = false;
                    NotBusy = !IsBusy;
                }
            }
            catch (Exception ex)
            {
                var x = ex;
                await _dialogService.ShowDialog(
                    ex.Message,
                    "Error logging you in",
                    "OK");
                IsBusy = false;
                NotBusy = !IsBusy;
            }

        }

    }
}
