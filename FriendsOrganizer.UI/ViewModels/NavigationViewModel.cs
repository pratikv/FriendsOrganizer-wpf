using System.Collections;
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
            _eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(FriendDetailViewModel):
                    var friend = Friends.SingleOrDefault(f => f.Id == args.Id);
                    if (friend != null)
                    {
                        Friends.Remove(friend);
                    }

                    break;
            }
        }

        private void AfterDetailSaved(AfterSavedEventArgs obj)
        {
            switch (obj.ViewModelName)
            {
                case nameof(FriendDetailViewModel):
                    NavigationItemViewModel lookupItem = Friends.SingleOrDefault(s => s.Id == obj.Id);
                    if (lookupItem == null)
                    {
                        Friends.Add(new NavigationItemViewModel(
                            obj.Id,
                            obj.DisplayMember,
                            nameof(FriendDetailViewModel),
                            _eventAggregator)
                        );
                    }
                    else
                    {
                        lookupItem.DisplayMember = obj.DisplayMember;
                    }

                    break;
            }

        }

        public async Task LoadAsync()
        {
            var lookup = await _friendLookupService.GetFrienLookupAsync();
            Friends.Clear();
            foreach (var item in lookup)
            {
                Friends.Add(new NavigationItemViewModel(
                    item.Id, 
                    item.DisplayMember,
                    nameof(FriendDetailViewModel),
                    _eventAggregator));
            }
        }

        public ObservableCollection<NavigationItemViewModel> Friends { get; set; }
    }
}