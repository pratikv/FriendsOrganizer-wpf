using System.Windows.Input;
using FriendsOrganizer.UI.Event;
using Prism.Commands;
using Prism.Events;

namespace FriendsOrganizer.UI.ViewModels
{
    public class NavigationItemViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        private string _displayMember;
        public int Id { get; }

        public string DisplayMember
        {
            get => _displayMember;
            set
            {
                _displayMember = value;
                OnPropertyChanged();
            }
        }

        public NavigationItemViewModel(int id, string displayMember, IEventAggregator _eventAggregator)
        {
            this._eventAggregator = _eventAggregator;
            Id = id;
            DisplayMember = displayMember;
            OpenFriendDetailViewCommand = new DelegateCommand(OnOpenFriendDetailView);
        }

        private void OnOpenFriendDetailView()
        {
            _eventAggregator.GetEvent<OpenFriendDetailViewEvent>().Publish(Id);
        }
        public ICommand OpenFriendDetailViewCommand { get; }

    }
}