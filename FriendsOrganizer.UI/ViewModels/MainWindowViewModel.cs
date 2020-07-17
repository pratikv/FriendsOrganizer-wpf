using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FriendsOrganizer.Model;
using FriendsOrganizer.UI.Event;
using FriendsOrganizer.UI.View.Services;
using Prism.Commands;
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
        private IDetailViewModel _detailViewModel;
        public INavigationViewModel NavigationViewModel { get; }

        public MainViewModel(INavigationViewModel navigationViewModel, Func<IFriendDetailViewModel> friendDetailViewModelCreator,
            IEventAggregator _eventAggregator, IMessageDialogService messageDialog)
        {
            _friendDetailViewModelCreator = friendDetailViewModelCreator;
            this._eventAggregator = _eventAggregator;
            _messageDialog = messageDialog;

            this._eventAggregator.GetEvent<OpenDetailViewEvent>()
                .Subscribe(OnOpenDetailView);
            this._eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
            NavigationViewModel = navigationViewModel;
            CreateNewDetailCommand = new DelegateCommand<Type>(OnCreateNewDetailExecute);
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            DetailViewModel = null;
        }

        private void OnCreateNewDetailExecute(Type viewModelType)
        {
            OnOpenDetailView(new OpenDetailViewEventArgs(){ViewModelName = viewModelType.Name});
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        public IDetailViewModel DetailViewModel
        {
            get => _detailViewModel;
            private set { _detailViewModel = value; OnPropertyChanged();}
        }


        private async void OnOpenDetailView(OpenDetailViewEventArgs args)
        {
            if (DetailViewModel != null && DetailViewModel.HasChanges)
            {
                var res = this._messageDialog.ShowOkCancelDialog("You have made changes. Navigate away?", "Question");
                if (res == MessageDialogResult.Ok)
                {
                    return;
                }
            }

            switch (args.ViewModelName)
            {
                case nameof(FriendDetailViewModel):
                    DetailViewModel = _friendDetailViewModelCreator();
                    break;
            }
            await DetailViewModel.LoadAsync(args.Id);
        }

        public ICommand CreateNewDetailCommand { get; set; }
    }

    public interface INavigationViewModel
    {
        Task LoadAsync();
    }
}