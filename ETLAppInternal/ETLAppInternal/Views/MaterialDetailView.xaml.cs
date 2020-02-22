using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace ETLAppInternal.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MaterialDetailView : ContentPage
	{
	    public MaterialDetailView()
	    {
	        InitializeComponent();
            
            if (Device.Idiom == TargetIdiom.Tablet)
            {
                Header.HeightRequest = 125;
                // Material.RowHeight = 65;
                SaveMaterial.HeightRequest = 65;
                ViewSamples.HeightRequest = 65;
            }
            else
            {
                Header.HeightRequest = 50;
                // Material.RowHeight = 75;
            }
        }
    }
}