using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ETLAppInternal.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginView : ContentPage
	{
		public LoginView()
		{
			InitializeComponent ();
		    if (Device.Idiom == TargetIdiom.Tablet)
		    {
		        LoginRow.Height = 400;
		        LoginColumn.Width = 350;
		        AbsoluteLayout.HeightRequest = 600;
		        AbsoluteLayout.WidthRequest = 400;
		    }
		    else
		    {
		        LoginRow.Height = 400;
		        LoginColumn.Width = 300;
		        AbsoluteLayout.HeightRequest = 600;
		        AbsoluteLayout.WidthRequest = 200;
            }
		}
	}
}