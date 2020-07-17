using System.Windows.Input;
using FriendsOrganizer.UI.Event;
using Prism.Commands;
using Prism.Events;

namespace FriendsOrganizer.UI.ViewModels
{
    public class NavigationItemViewModel : ViewModelBase
    {
        private readonly string _detailViewModelName;
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

        public NavigationItemViewModel(int id, string displayMember, string detailViewModelName, IEventAggregator _eventAggregator)
        {
            _detailViewModelName = detailViewModelName;
            this._eventAggregator = _eventAggregator;
            Id = id;
            DisplayMember = displayMember;
            OpenDetailViewCommand = new DelegateCommand(OnOpenDetailViewExecute);
        }

        private void OnOpenDetailViewExecute()
        {
            _eventAggregator.GetEvent<OpenDetailViewEvent>().Publish(
                new OpenDetailViewEventArgs()
                {
                    Id = Id,
                    ViewModelName = _detailViewModelName
                });
        }
        public ICommand OpenDetailViewCommand { get; }

    }
}