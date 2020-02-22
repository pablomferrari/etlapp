using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ETLAppInternal.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SampleDetailView : ContentPage
	{
		public SampleDetailView ()
		{
			InitializeComponent ();

		    if (Device.Idiom == TargetIdiom.Tablet)
		    {
		        Header.HeightRequest = 125;
                SaveButton.HeightRequest = 75;
                // Sample.RowHeight = 65;
            }
		    else
		    {
		        Header.HeightRequest = 50;
		        // Sample.RowHeight = 55;
		    }
        }
	}
}