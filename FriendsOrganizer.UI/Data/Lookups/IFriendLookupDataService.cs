using System.Collections.Generic;
using System.Threading.Tasks;
using FriendsOrganizer.Model;

namespace FriendsOrganizer.UI.Data
{
    public interface IFriendLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetFrienLookupAsync();
    }
}