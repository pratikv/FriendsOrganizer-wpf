using System.Threading.Tasks;

namespace FriendsOrganizer.UI.ViewModels
{
    public interface IFriendDetailViewModel
    {
        Task LoadAsync(int? friendId);
        bool HasChanges { get; }
    }
}