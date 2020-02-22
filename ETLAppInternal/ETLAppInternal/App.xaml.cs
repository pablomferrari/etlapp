using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Akavache;
using ETLAppInternal.Bootstrap;
using ETLAppInternal.Constants;
using ETLAppInternal.Contracts.Services.Data;
using ETLAppInternal.Contracts.Services.General;
using ETLAppInternal.Data;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Distribute;
using Microsoft.AppCenter.Push;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ETLAppInternal
{
	public partial class App : Application
	{
        static AsbestosSurveyDatabase database;

        public static AsbestosSurveyDatabase Database
        {
            get
            {
                if(database == null)
                {
                    database = new AsbestosSurveyDatabase();
                }
                return database;
            }
        }
		
		public App ()
		{
		    InitializeComponent();
            InitializeApp();
            InitializeNavigation().ConfigureAwait(false);
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("OTcwOUAzMTM2MmUzMjJlMzBCRkpoaGVKelJVTGxXNUxsNG16VnFWamIvUmp2YitSVi9hM0VuVDdGQy9JPQ==");
		    BlobCache.ApplicationName = "ETC_ETL_Sample_Submission";
			AppCenter.Start("android=afd9a230-9c69-425e-9897-c08228c6bcf3;ios=fc691dca-2251-4d46-b990-ba2398ac763e;", 
                typeof(Analytics), typeof(Crashes), typeof(Distribute), typeof(Push));
        }

	    private async Task InitializeNavigation()
	    {
	        var navigationService = AppContainer.Resolve<INavigationService>();
	        await navigationService.InitializeAsync();
	    }

	    private void InitializeApp()
	    {
	        AppContainer.RegisterDependencies();
        }

        protected override void OnStart ()
		{

        }

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
