using System.Threading.Tasks;
using System.Windows.Input;
using FriendsOrganizer.Model;
using FriendsOrganizer.UI.Data;
using FriendsOrganizer.UI.Data.Repositories;
using FriendsOrganizer.UI.Event;
using FriendsOrganizer.UI.Wrapper;
using Prism.Commands;
using Prism.Events;

namespace FriendsOrganizer.UI.ViewModels
{
    public class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IEventAggregator _eventAggregator;
        private FriendWrapper _friend;

        public FriendDetailViewModel(IFriendRepository friendRepository, IEventAggregator eventAggregator)
        {
            _friendRepository = friendRepository;
            _eventAggregator = eventAggregator;
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecucte);
        }

        private bool OnSaveCanExecucte()
        {
            return Friend!=null && !Friend.HasErrors && HasChanges;
        }

        private async void OnSaveExecute()
        {
            await _friendRepository.SaveAsync();
            HasChanges = _friendRepository.HasChanges();
            _eventAggregator.GetEvent<AfterFriendSavedEvent>().Publish(new AfterFriendSavedEventArgs()
            {
                Id = Friend.Id,
                DisplayMember = Friend.FirstName
            });
        }

        private bool _hasChanges;

        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged(); 
                }
            }
        }

        public async Task LoadAsync(int friendId)
        {
            var friend = await _friendRepository.GetByIdAsync(friendId);
            Friend = new FriendWrapper(friend);
            Friend.PropertyChanged += (sender, args) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _friendRepository.HasChanges();
                }
                if (args.PropertyName == nameof(Friend.HasErrors))
                {
                    ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        public FriendWrapper Friend
        {
            get => _friend;
            set
            {
                _friend = value;
                base.OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; }
    }
}