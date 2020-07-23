using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Windows.Input;
using FriendsOrganizer.Model;
using FriendsOrganizer.UI.Data.Repositories;
using FriendsOrganizer.UI.Event;
using FriendsOrganizer.UI.View.Services;
using FriendsOrganizer.UI.Wrapper;
using Prism.Commands;
using Prism.Events;

namespace FriendsOrganizer.UI.ViewModels
{
    public interface IMeetingDetailViewModel : IDetailViewModel
    {
    }

    public class MeetingDetailViewModel : DetailViewModelBase,IMeetingDetailViewModel
    {
        private readonly IMeetingRepository _meetingRepository;
        private MeetingWrapper _meeting;
        private Friend _selectedAddedFriend;
        private Friend _selectedAvailableFriend;
        private List<Friend> _allFreinds;

        public MeetingDetailViewModel(
            IEventAggregator _eventAggregator,
            IMessageDialogService messageDialogService,
            IMeetingRepository meetingRepository
            ) : base(_eventAggregator,messageDialogService)
        {
            _meetingRepository = meetingRepository;
            
            _eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
            
            AddedFriends = new ObservableCollection<Friend>();
            AvailableFriends = new ObservableCollection<Friend>();
            AddFriendCommand = new DelegateCommand(OnAddFriendExecute, OnAddFriendCanExecute);
            RemoveFriendCommand = new DelegateCommand(OnRemoveFriendExecute, OnRemoveFriendCanExecute);
        }

        private async void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            if (args.ViewModelName == nameof(FriendDetailViewModel))
            {
                _allFreinds = await _meetingRepository.GetAllFriendsAsync();

                SetupPickList();
            }
        }

        private async void AfterDetailSaved(AfterSavedEventArgs args)
        {
            if (args.ViewModelName == nameof(FriendDetailViewModel))
            {
                await _meetingRepository.ReloadFriendAsync(args.Id);
                _allFreinds = await _meetingRepository.GetAllFriendsAsync();

                SetupPickList();
            }
        }

        public ICommand RemoveFriendCommand { get; }

        private void OnRemoveFriendExecute()
        {
            var friendToRemove = SelectedAddedFriend;
            Meeting.Model.Friends.Remove(friendToRemove);
            AddedFriends.Remove(friendToRemove);
            AvailableFriends.Add(friendToRemove);
            HasChanges = _meetingRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnRemoveFriendCanExecute()
        {
            return SelectedAddedFriend != null;
        }

        public ICommand AddFriendCommand { get; }

        private bool OnAddFriendCanExecute()
        {
            return SelectedAvailableFriend != null;
        }

        private void OnAddFriendExecute()
        {
            var friendToAdd = SelectedAvailableFriend;
            Meeting.Model.Friends.Add(friendToAdd);
            AddedFriends.Add(friendToAdd);
            AvailableFriends.Remove(friendToAdd);
            HasChanges = _meetingRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        public Friend SelectedAddedFriend
        {
            get => _selectedAddedFriend;
            set
            {
                _selectedAddedFriend = value;
                OnPropertyChanged();
                ((DelegateCommand) RemoveFriendCommand).RaiseCanExecuteChanged();
            }
        }

        public Friend SelectedAvailableFriend
        {
            get => _selectedAvailableFriend;
            set
            {
                _selectedAvailableFriend = value;
                OnPropertyChanged();
                ((DelegateCommand) AddFriendCommand).RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<Friend> AvailableFriends { get; set; }

        public ObservableCollection<Friend> AddedFriends { get; set; }

        public MeetingWrapper Meeting
        {
            get { return _meeting; }
            set { _meeting = value; OnPropertyChanged(); }
        }

        protected override void OnDeleteExecute()
        {
            var result =
                _messageDialogService.ShowOkCancelDialog($"Do you really want to delete the meeting?", "Question");
            if (result == MessageDialogResult.Cancel)
            {
                return;
            }
            _meetingRepository.Remove(Meeting.Model);
            _meetingRepository.SaveAsync();
            RaiseDetailDeletedEvent(Meeting.Id);
        }

        protected override bool OnSaveCanExecute()
        {
            return Meeting != null && !Meeting.HasErrors && HasChanges;
        }

        protected override async void OnSaveExecute()
        {
            await _meetingRepository.SaveAsync();
            HasChanges = _meetingRepository.HasChanges();
            Id = Meeting.Id;
            RaiseDetailSavedEvent(Meeting.Id, Meeting.Title);
        }

        public override async Task LoadAsync(int id)
        {
            var meeting =
                id > 0 ? await _meetingRepository.GetByIdAsync(id) : CreateNewMeeting();
            Id = id;
            InitializeMeeting(meeting);

            _allFreinds = await _meetingRepository.GetAllFriendsAsync();

            SetupPickList();
        }

        private void SetupPickList()
        {
            var meetingFriendIds = Meeting.Model.Friends.Select(f => f.Id).ToList();
            var addedFriends = _allFreinds.Where(f => meetingFriendIds.Contains(f.Id)).OrderBy(f => f.FirstName);
            var availableFriends = _allFreinds.Except(addedFriends).OrderBy(f => f.FirstName);

            AddedFriends.Clear();
            AvailableFriends.Clear();

            foreach (var addedFriend in addedFriends)
            {
                AddedFriends.Add(addedFriend);
            }

            foreach (var availableFriend in availableFriends)
            {
                AvailableFriends.Add(availableFriend);
            }
        }

        private void InitializeMeeting(Meeting meeting)
        {
            Meeting = new MeetingWrapper(meeting);
            Meeting.PropertyChanged += (sender, args) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _meetingRepository.HasChanges();
                }

                if (args.PropertyName == nameof(Meeting.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }

                if (args.PropertyName == nameof(Meeting.Title))
                {
                    SetTitle();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            if (Meeting.Id == 0)
            {
                Meeting.Title = "";
            }

            SetTitle();
        }

        private void SetTitle()
        {
            Title = $"{Meeting.Title}";
        }

        private Meeting CreateNewMeeting()
        {
            var meeting = new Meeting()
            {
                DateFrom = DateTime.Now.Date,
                DateTo = DateTime.Now.Date
            };
            _meetingRepository.Add(meeting);
            return meeting;
        }
    }
}