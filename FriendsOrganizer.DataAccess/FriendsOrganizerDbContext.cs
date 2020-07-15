using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FriendsOrganizer.Model;

namespace FriendsOrganizer.DataAccess
{
    public class FriendsOrganizerDbContext : DbContext
    {
        public FriendsOrganizerDbContext()
        :base("FriendsOrganizerDb")
        {
        }

        public DbSet<Friend> Friends { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
