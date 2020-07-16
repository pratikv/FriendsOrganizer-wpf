using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FriendsOrganizer.DataAccess;
using FriendsOrganizer.Model;

namespace FriendsOrganizer.UI.Data
{
    public interface IProgrammingLanguageLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetProgrammingLanguageLookupAsync();
    }

    public class LookupDataService : IFriendLookupDataService, IProgrammingLanguageLookupDataService
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

        public async Task<IEnumerable<LookupItem>> GetProgrammingLanguageLookupAsync()
        {
            using (var ctx = _context())
            {
                return await ctx.ProgrammingLanguages.AsNoTracking()
                    .Select(s => new LookupItem()
                    {
                        Id = s.Id,
                        DisplayMember = s.Name
                    }).ToListAsync();
            }
        }

    }
}