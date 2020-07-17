using System.Data.Entity.Core.Common.CommandTrees;
using System.Threading.Tasks;
using System.Windows.Input;
using FriendsOrganizer.UI.Event;
using Prism.Commands;
using Prism.Events;

namespace FriendsOrganizer.UI.ViewModels
{
    public interface IDetailViewModel
    {
        Task LoadAsync(int? id);
        bool HasChanges { get; }
    }

    public interface IFriendDetailViewModel : IDetailViewModel
    {

    }

    public abstract class DetailViewModelBase : ViewModelBase, IDetailViewModel
    {
        protected readonly IEventAggregator EventAggregator;
        private bool _hasChanges;

        public DetailViewModelBase(IEventAggregator _eventAggregator)
        {
            this.EventAggregator = _eventAggregator;

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
        }

        protected abstract void OnDeleteExecute();

        protected abstract bool OnSaveCanExecute();

        protected abstract void OnSaveExecute();

        public abstract Task LoadAsync(int? id);

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