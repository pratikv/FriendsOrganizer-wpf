using System.Threading.Tasks;
using System.Windows.Input;
using FriendsOrganizer.UI.Event;
using FriendsOrganizer.UI.View.Services;
using Prism.Commands;
using Prism.Events;

namespace FriendsOrganizer.UI.ViewModels
{
    public abstract class DetailViewModelBase : ViewModelBase, IDetailViewModel
    {
        protected readonly IEventAggregator EventAggregator;
        protected readonly IMessageDialogService _messageDialogService;
        private bool _hasChanges;
        private int _id;
        private string _title;

        public DetailViewModelBase(IEventAggregator _eventAggregator, IMessageDialogService messageDialogService)
        {
            this.EventAggregator = _eventAggregator;
            _messageDialogService = messageDialogService;

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
            CloseDetailViewCommand = new DelegateCommand(OnCloseDetailViewExecute);
        }

        protected virtual void OnCloseDetailViewExecute()
        {
            if (HasChanges)
            {
                var result = _messageDialogService.ShowOkCancelDialog(
                    "You have some unsaved changes. Are you sure, you want to close?", 
                    "Question"
                    );
                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }
            EventAggregator.GetEvent<AfterDetailClosedEvent>().Publish(new AfterDetailClosedEventArgs()
            {
                Id = this.Id,
                ViewModelName = this.GetType().Name
            });
        }

        public ICommand CloseDetailViewCommand { get; }

        protected abstract void OnDeleteExecute();

        protected abstract bool OnSaveCanExecute();

        protected abstract void OnSaveExecute();

        public abstract Task LoadAsync(int i);

        public string Title
        {
            get => _title;
            protected set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public int Id
        {
            get => _id;
            protected set => _id = value;
        }


        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                if(_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        protected virtual void RaiseDetailDeletedEvent(int modelId)
        {
            EventAggregator
                .GetEvent<AfterDetailDeletedEvent>()
                .Publish(new AfterDetailDeletedEventArgs()
                {
                    Id = modelId, 
                    ViewModelName = this.GetType().Name
                });
        }

        protected virtual void RaiseDetailSavedEvent(int modelId, string displayMember)
        {
            EventAggregator.GetEvent<AfterDetailSavedEvent>().Publish(new AfterSavedEventArgs()
            {
                Id = modelId,
                DisplayMember = displayMember,
                ViewModelName = this.GetType().Name
            });
        }
    }
}