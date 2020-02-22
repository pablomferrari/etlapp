using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ETLAppInternal.Contracts.Services.Data;
using ETLAppInternal.Contracts.Services.General;
using ETLAppInternal.Extensions;
using ETLAppInternal.Models.General;
using ETLAppInternal.Models.Jobs;
using ETLAppInternal.ViewModels.Base;
using Xamarin.Forms;

namespace ETLAppInternal.ViewModels
{
    public class DeliveriesViewModel : ViewModelBase
    {
        private readonly ISqlLiteService _dataService;
        private readonly ISettingsService _settingsService;
        private int JobId { get; set; }

        public ObservableCollection<PlmInstructions> _plmInstructions;
        public ObservableCollection<PlmInstructions> PlmInstructions
        {
            get => _plmInstructions;
            set
            {
                _plmInstructions = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<string> _turnAroundCollection;
        public ObservableCollection<string> TurnAroundOptions
        {
            get => _turnAroundCollection;
            set
            {
                _turnAroundCollection = value;
                OnPropertyChanged();
            }
        }

        private bool _canSave;

        public bool CanSave
        {
            get => _canSave;
            set { _canSave = value; OnPropertyChanged(); }
        }

        private string _turnAround;
        public string TurnAround
        {
            get => _turnAround;
            set
            {
                _turnAround = value; OnPropertyChanged();
                CanSave = !string.IsNullOrEmpty(TurnAround); 
            }
        }

        private string _title;

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public DeliveriesViewModel(
            IConnectionService connectionService, 
            INavigationService navigationService, 
            IDialogService dialogService, ISqlLiteService dataService, ISettingsService settingsService) : base(connectionService, navigationService, dialogService)
        {
            _dataService = dataService;
            _settingsService = settingsService;
        }

        public ICommand SelectedCommand => new Command(ItemSelected);

        public ICommand OnSaveChangesCommand => new Command(SaveDelivery);

        private async void SaveDelivery()
        {
            var optionId = TurnAroundHelper.GetOptionId(TurnAround);
            var selectedInstructions = PlmInstructions.Where(x => x.Selected).ToList();
            var selectedInstructionStrings =
                selectedInstructions.Select(x => PlmInstructionsHelper.GetOption(x.Name)).ToList();
            var deliveryRequest = new DeliveryRequest
            {
                JobId = JobId,
                StatusId = 5,
                TurnAround = optionId,
                PlmInstructions = string.Join(";", selectedInstructionStrings),
                EmployeeId = string.IsNullOrEmpty(_settingsService.UserIdSetting) ? 0 : int.Parse(_settingsService.UserIdSetting)
            };
            await _dataService.AddDelivery(deliveryRequest.ToDeliveryRequestTable());
            await _dialogService.ShowDialog(
                "Chain of custody data has been set",
                "Delivery",
                "OK");
            await _navigationService.PopToRootAsync();
        }

        private void ItemSelected(object obj)
        {
            var data = (PlmInstructions)obj;
            var index = PlmInstructions.First(x => x.Name == data.Name);
            index.Selected = !data.Selected;
        }

        public override async Task InitializeAsync(object data)
        {
            await Task.Run(() =>
            {
                IsBusy = true;
                JobId = (int) data;
                Title = "Job #: " + JobId;
                PlmInstructions = PlmInstructionsHelper.GetInstructions().ToObservableCollection();
                TurnAroundOptions = TurnAroundHelper.TurnAroundOptions.ToObservableCollection();
                IsBusy = false;
            });
        }
    }
}
