using System.Threading.Tasks;

namespace FriendsOrganizer.UI.ViewModels
{
    public interface IDetailViewModel
    {
        Task LoadAsync(int id);
        bool HasChanges { get; }
        int Id { get; }
    }
}