using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FriendsOrganizer.DataAccess;
using FriendsOrganizer.Model;

namespace FriendsOrganizer.UI.Data
{
    public class LookupDataService : IFriendLookupDataService
    {
        private readonly Func<FriendsOrganizerDbContext> _context;

        public LookupDataService(Func<FriendsOrganizerDbContext> context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LookupItem>> GetFrienLookupAsync()
        {
            using (var ctx = _context())
            {
                return await ctx.Friends.AsNoTracking()
                    .Select(s => new LookupItem()
                    {
                        Id = s.Id,
                        DisplayMember = s.FirstName
                    }).ToListAsync();
            }
        }
    }
}