using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ETLAppInternal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeliveriesView : ContentPage
    {
        public DeliveriesView()
        {
            InitializeComponent();
            if (Device.Idiom == TargetIdiom.Tablet)
            {
                SaveButton.HeightRequest = 75;
                // Sample.RowHeight = 65;
            }
        }
    }
}