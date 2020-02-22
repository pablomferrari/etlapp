using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ETLAppInternal.Contracts.Services.Data;
using ETLAppInternal.Contracts.Services.General;
using ETLAppInternal.Extensions;
using ETLAppInternal.Models.Samples;
using ETLAppInternal.Models.Sql;
using ETLAppInternal.ViewModels.Base;
using Xamarin.Forms;

namespace ETLAppInternal.ViewModels
{
    public class SampleDetailViewModel : ViewModelBase
    {
        #region properties
        public Samples CurrentSample;
        public string _clientSampleId;
        public string ClientSampleId
        {
            get => _clientSampleId;
            set
            {
                _clientSampleId = value;
                OnPropertyChanged();
            }
        }
        public string _sampleLocation;

        public string SampleLocation
        {
            get => _sampleLocation;
            set
            {
                _sampleLocation = value;
                OnPropertyChanged();
            }
        }
        #endregion
        private readonly ISqlLiteService _dataService;
        public SampleDetailViewModel(IConnectionService connectionService, 
            INavigationService navigationService, 
            IDialogService dialogService,
            ISqlLiteService dataService) 
            : base(connectionService, navigationService, dialogService)
        {
            _dataService = dataService;
        }

        public ICommand OnSaveChangesCommand => new Command(SaveSample);


        private Samples BuildSample()
        {
            var sample = new Samples
            {
                Id = CurrentSample.Id,
                JobId = CurrentSample.JobId,
                MaterialId = CurrentSample.MaterialId,
                SampleLocation = SampleLocation,
                ClientSampleId = ClientSampleId,
                DateCollected = !string.IsNullOrEmpty(CurrentSample.DateCollected) ?
                    CurrentSample.DateCollected :
                new DateTime().ToShortDateString(),
                Delete = CurrentSample.Delete,
                IsLocal = CurrentSample.IsLocal,
                IsNew = CurrentSample.IsNew,
                SampleDescription = CurrentSample.SampleDescription 
            };
            return sample;
        }

        private async void SaveSample()
        {
            CurrentSample.IsLocal = true;
            await _dataService.InsertOrUpdateAsync<SamplesTable>(BuildSample().ToSamplesTable());
            await _navigationService.NavigateBackAsync();
            MessagingCenter.Send(this, Constants.MessengerConstants.SampleDetailViewModel_SampleAdded);
        }

        public override async Task InitializeAsync(object data)
        {
            await Task.Run(() =>
            {
                CurrentSample = (Samples)data;
                ClientSampleId = CurrentSample.ClientSampleId;
                SampleLocation = CurrentSample.SampleLocation;
            });
        }
    }
}
