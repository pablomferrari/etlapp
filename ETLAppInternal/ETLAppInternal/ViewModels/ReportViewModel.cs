using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ETLAppInternal.Contracts.Services.Data;
using ETLAppInternal.Contracts.Services.General;
using ETLAppInternal.Models.Jobs;
using ETLAppInternal.ViewModels.Base;
using Xamarin.Forms;

namespace ETLAppInternal.ViewModels
{
    public class ReportViewModel: ViewModelBase
    {
        private readonly ISqlLiteService _dataService;
        public ReportViewModel(IConnectionService connectionService,
            INavigationService navigationService,
            IDialogService dialogService, ISqlLiteService dataService) : base(connectionService,
            navigationService,
            dialogService)
        {
            _dataService = dataService;
        }

        public ICommand DoneCommand => new Command(async () => { await _navigationService.NavigateBackAsync(); });

        private string _header;

        public string Header
        {
            get => _header;
            set { _header = value; OnPropertyChanged(); }
        }

        private string _detail;

        public string Detail
        {
            get => _detail;
            set { _detail = value; OnPropertyChanged(); }
        }
        public override async Task InitializeAsync(object data)
        {
            await Task.Run(async () =>
            {
                IsBusy = true;
                var job = (Jobs) data;
                Header = job.JobId + " " + job.Client.Name;
                var materials = (await _dataService.GetMaterialsAsync(job.JobId)).ToList();
                foreach (var material in materials)
                {
                    material.Samples = (await _dataService.GetSamplesAsync(material.Id)).ToList();
                }

                var sb = new StringBuilder();
                foreach (var material in materials)
                {
                    sb.AppendLine($"{material.ClientMaterialId} {material.Description} " +
                                  $"{(material.IsLocal ? "***" : "")}");
                                  
                    sb.AppendLine("------------------------------------------------------------");
                    if (material.Samples == null || !material.Samples.Any())
                    {
                        sb.AppendLine();
                        continue;
                    }
                    foreach (var sample in material.Samples.OrderBy(x => x.ClientSampleId))
                    {
                        sb.AppendLine($"{sample.ClientSampleId} {sample.SampleLocation} " +
                                      $"{(sample.IsLocal ? "***" : "")}");
                    }

                    sb.AppendLine();
                }

                Detail = sb.ToString();
                IsBusy = false;
            });
        }
    }
}
