using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ETLAppInternal.Constants;
using ETLAppInternal.Contracts.Services.General;
using ETLAppInternal.Extensions;
using ETLAppInternal.Models.Materials;
using ETLAppInternal.ViewModels.Base;
using Xamarin.Forms;

namespace ETLAppInternal.ViewModels
{
    public class RoomsViewModel : ViewModelBase
    {
        private ObservableCollection<Room> _rooms;
        private ObservableCollection<Room> _filteredRooms;
        public ObservableCollection<Room> FilteredRooms
        {
            get => _filteredRooms;
            set
            {
                _filteredRooms = value;
                OnPropertyChanged();
            }
        }
        private string _search;
        public string Search
        {
            get => _search;
            set
            {
                _search = value;
                FilterChanged(value);
                OnPropertyChanged();
            }
        }

        private void FilterChanged(string value)
        {
            if (string.IsNullOrEmpty(value))
                FilteredRooms = _rooms;
            else
            {
                FilteredRooms = _rooms.Where(x =>
                    x.Name.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0
                ).ToObservableCollection();
            }
        }

        public ICommand SelectedCommand => new Command(ItemSelected);
        public ICommand DoneCommand => new Command(RoomsDone);

        private void RoomsDone()
        {
            var rooms = _rooms.Where(x => x.Selected).Select(x => x.Name).ToList();
            MessagingCenter.Send(this, MessengerConstants.RoomsViewModel_RoomsAdded, rooms);
            _navigationService.NavigateBackAsync();
        }

        private void ItemSelected(object obj)
        {
            var data = (Room)obj;
            var index = FilteredRooms.First(x => x.Name == data.Name);
            index.Selected = !data.Selected;
        }

        public RoomsViewModel(IConnectionService connectionService, INavigationService navigationService,
            IDialogService dialogService)
            : base(connectionService, navigationService, dialogService)
        {
        }

        public override async Task InitializeAsync(object data)
        {
            await Task.Run(() =>
            {
                IsBusy = true;
                var rooms = MaterialHelper.GetRooms();
                _rooms = rooms.Select(x => new Room { Name = x.Name, Selected = false }).ToObservableCollection();
                if (!string.IsNullOrEmpty(data?.ToString()))
                {
                    var selectedRooms = data.ToString().Split(',');
                    foreach (var room in selectedRooms)
                    {
                        var index = _rooms.First(x => x.Name == room);
                        index.Selected = true;
                    }
                }
                FilteredRooms = _rooms;
                IsBusy = false;
            });
           
        }

    }
}
