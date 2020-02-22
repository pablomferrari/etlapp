using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ETLAppInternal.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ETLNavigationPage : NavigationPage
	{
		public ETLNavigationPage ()
		{
			InitializeComponent ();
		}

	    public ETLNavigationPage(Page root) : base(root)
	    {
	        InitializeComponent();
	    }
    }
}