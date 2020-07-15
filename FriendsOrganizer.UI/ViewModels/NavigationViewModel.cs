using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FriendsOrganizer.UI.Data;
using FriendsOrganizer.UI.Event;
using Prism.Events;

namespace FriendsOrganizer.UI.ViewModels
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private readonly IFriendLookupDataService _friendLookupService;
        private readonly IEventAggregator _eventAggregator;

        public NavigationViewModel(IFriendLookupDataService friendLookupService, IEventAggregator eventAggregator)
        {
            _friendLookupService = friendLookupService;
            _eventAggregator = eventAggregator;
            Friends = new ObservableCollection<NavigationItemViewModel>();
            _eventAggregator.GetEvent<AfterFriendSavedEvent>().Subscribe(AfterFriendSaved);
        }

        private void AfterFriendSaved(AfterFriendSavedEventArgs obj)
        {
            var lookupItem = Friends.Single(s => s.Id == obj.Id);
            lookupItem.DisplayMember = obj.DisplayMember;
        }

        public async Task LoadAsync()
        {
            var lookup = await _friendLookupService.GetFrienLookupAsync();
            Friends.Clear();
            foreach (var item in lookup)
            {
                Friends.Add(new NavigationItemViewModel(item.Id, item.DisplayMember,_eventAggregator));
            }
        }

        public ObservableCollection<NavigationItemViewModel> Friends { get; set; }
        private NavigationItemViewModel _selectedFriend;

        public NavigationItemViewModel SelectedFriend
        {
            get { return _selectedFriend; }
            set
            {
                _selectedFriend = value;
                OnPropertyChanged();
                if (_selectedFriend != null)
                {
                    _eventAggregator.GetEvent<OpenFriendDetailViewEvent>().Publish(_selectedFriend.Id); 
                }
            }
        }

    }
}