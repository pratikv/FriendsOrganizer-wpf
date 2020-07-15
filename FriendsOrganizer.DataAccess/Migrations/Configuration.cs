using FriendsOrganizer.Model;

namespace FriendsOrganizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FriendsOrganizer.DataAccess.FriendsOrganizerDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FriendsOrganizer.DataAccess.FriendsOrganizerDbContext context)
        {
            context.Friends.AddOrUpdate(f => f.FirstName,
                new Friend() {FirstName = "Pratk", LastName = "Vakil"},
                new Friend() {FirstName = "Bruce", LastName = "Wayne"},
                new Friend() {FirstName = "Clark", LastName = "Kent"}
            );
        }
    }
}
