using System;
using System.Threading.Tasks;
using System.Windows;
using FriendsOrganizer.Model;
using FriendsOrganizer.UI.Event;
using FriendsOrganizer.UI.View.Services;
using Prism.Events;

namespace FriendsOrganizer.UI.ViewModels
{
    /// <summary>
    /// Description of MainViewModel.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly Func<IFriendDetailViewModel> _friendDetailViewModelCreator;
        private readonly IEventAggregator _eventAggregator;
        private readonly IMessageDialogService _messageDialog;
        private IFriendDetailViewModel _friendDetailViewModel;
        public INavigationViewModel NavigationViewModel { get; }

        public MainViewModel(INavigationViewModel navigationViewModel, Func<IFriendDetailViewModel> friendDetailViewModelCreator,
            IEventAggregator _eventAggregator, IMessageDialogService messageDialog)
        {
            _friendDetailViewModelCreator = friendDetailViewModelCreator;
            this._eventAggregator = _eventAggregator;
            _messageDialog = messageDialog;

            _eventAggregator.GetEvent<OpenFriendDetailViewEvent>()
                .Subscribe(OnOpenFriendDetailView);
            
            NavigationViewModel = navigationViewModel;

        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        public IFriendDetailViewModel FriendDetailViewModel
        {
            get => _friendDetailViewModel;
            private set { _friendDetailViewModel = value; OnPropertyChanged();}
        }


        private async void OnOpenFriendDetailView(int friendId)
        {
            if (FriendDetailViewModel != null && FriendDetailViewModel.HasChanges)
            {
                var res = this._messageDialog.ShowOkCancelDialog("You have made changes. Navigate away?", "Question");
                if (res == MessageDialogResult.Ok)
                {
                    return;
                }
            }
            FriendDetailViewModel = _friendDetailViewModelCreator();
            await FriendDetailViewModel.LoadAsync(friendId);
        }
    }

    public interface INavigationViewModel
    {
        Task LoadAsync();
    }
}