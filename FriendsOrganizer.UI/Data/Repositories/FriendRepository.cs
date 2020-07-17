using System;
using System.Data.Entity;
using System.Threading.Tasks;
using FriendsOrganizer.Model;
using FriendsOrganizer.DataAccess;

namespace FriendsOrganizer.UI.Data.Repositories
{

    public class GenericRepository<TEntity, TContext> : IGenericRepository<TEntity>
    where TContext : DbContext
    where TEntity: class
    {
        protected readonly TContext Context;

        protected GenericRepository(TContext context)
        {
            Context = context;
        }

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }

        public async Task SaveAsync()
        {
            await Context.SaveChangesAsync();
        }

        public bool HasChanges()
        {
            return Context.ChangeTracker.HasChanges();
        }

        public void Add(TEntity model)
        {
            Context.Set<TEntity>().Add(model);
        }

        public void Remove(TEntity model)
        {
            Context.Set<TEntity>().Remove(model);
        }
    }

    public class FriendRepository : GenericRepository<Friend, FriendsOrganizerDbContext>, IFriendRepository
    {
        public FriendRepository(FriendsOrganizerDbContext context)
            :base(context)
        {
        }

        public override async Task<Friend> GetByIdAsync(int friendId)
        {
            return await Context.Friends
                .Include(f => f.PhoneNumbers)
                .SingleAsync(s => s.Id == friendId);
        }

        public void RemovePhoneNumber(FriendPhoneNumber model)
        {
            Context.FriendPhoneNumbers.Remove(model);
        }
    }
}