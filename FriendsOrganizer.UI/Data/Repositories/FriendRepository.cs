using System;
using System.Data.Entity;
using System.Threading.Tasks;
using FriendsOrganizer.Model;
using FriendsOrganizer.DataAccess;

namespace FriendsOrganizer.UI.Data.Repositories
{
    /// <summary>
    /// Description of FriendRepository.
    /// </summary>
    public class FriendRepository : IFriendRepository
    {
        private readonly FriendsOrganizerDbContext _context;

        public FriendRepository(FriendsOrganizerDbContext context)
        {
            _context = context;
        }

        public async Task<Friend> GetByIdAsync(int friendId)
        {
            {
                return await _context.Friends
                    .Include(f => f.PhoneNumbers)
                    .SingleAsync(s => s.Id == friendId);
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }

        public void Add(Friend friend)
        {
            _context.Friends.Add(friend);
        }

        public void Remove(Friend friend)
        {
            _context.Friends.Remove(friend);
        }

        public void RemovePhoneNumber(FriendPhoneNumber model)
        {
            _context.FriendPhoneNumbers.Remove(model);
        }
    }
}