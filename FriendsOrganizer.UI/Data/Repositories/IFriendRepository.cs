using System.Threading.Tasks;
using FriendsOrganizer.Model;

namespace FriendsOrganizer.UI.Data.Repositories
{
    public interface IFriendRepository
    {
        Task<Friend> GetByIdAsync(int friendId);
        Task SaveAsync();
        bool HasChanges();
        void Add(Friend friend);
        void Remove(Friend friend);
        void RemovePhoneNumber(FriendPhoneNumber model);
    }
}