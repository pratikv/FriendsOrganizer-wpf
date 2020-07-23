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
        private readonly IMeetingLookupDataService _meetingLookupService;
        private readonly IEventAggregator _eventAggregator;

        public NavigationViewModel(
            IFriendLookupDataService friendLookupService,
            IMeetingLookupDataService meetingLookupService,
            IEventAggregator eventAggregator)
        {
            _friendLookupService = friendLookupService;
            _meetingLookupService = meetingLookupService;
            _eventAggregator = eventAggregator;
            Friends = new ObservableCollection<NavigationItemViewModel>();
            Meetings = new ObservableCollection<NavigationItemViewModel>();
            _eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(FriendDetailViewModel):
                    AfterDetailDeleted(Friends, args);
                    break;
                case nameof(MeetingDetailViewModel):
                    AfterDetailDeleted(Meetings, args);
                    break;

            }
        }

        private void AfterDetailDeleted(ObservableCollection<NavigationItemViewModel> items, AfterDetailDeletedEventArgs args)
        {
            var item = items.SingleOrDefault(f => f.Id == args.Id);
            if (item != null)
            {
                items.Remove(item);
            }
        }

        private void AfterDetailSaved(AfterSavedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(FriendDetailViewModel):
                    AfterDetailSaved(Friends, args);
                    break;
                case nameof(MeetingDetailViewModel):
                    AfterDetailSaved(Meetings, args);
                    break;
            }
        }

        private void AfterDetailSaved(ObservableCollection<NavigationItemViewModel> items, AfterSavedEventArgs args)
        {
            var lookupItem = items.SingleOrDefault(s => s.Id == args.Id);
            if (lookupItem == null)
            {
                items.Add(new NavigationItemViewModel(
                    args.Id,
                    args.DisplayMember,
                    args.ViewModelName,
                    _eventAggregator)
                );
            }
            else
            {
                lookupItem.DisplayMember = args.DisplayMember;
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

            lookup = await _meetingLookupService.GetMeetingLookupAsync();
            Meetings.Clear();
            foreach (var item in lookup)
            {
                Meetings.Add(new NavigationItemViewModel(
                    item.Id,
                    item.DisplayMember,
                    nameof(MeetingDetailViewModel),
                    _eventAggregator));
            }

        }

        public ObservableCollection<NavigationItemViewModel> Friends { get; set; }
        public ObservableCollection<NavigationItemViewModel> Meetings { get; set; }
    }
}