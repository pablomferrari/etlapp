using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ETLAppInternal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoomsView : ContentPage
    {
        public ObservableCollection<string> Items { get; set; }

        public RoomsView()
        {
            InitializeComponent();
            // ListView.ItemTapped += ItemTapped;
        }

    }
}
