using System;
using Autofac;
using FriendsOrganizer.DataAccess;
using FriendsOrganizer.UI.Data;
using FriendsOrganizer.UI.Data.Repositories;
using FriendsOrganizer.UI.View.Services;
using FriendsOrganizer.UI.ViewModels;
using Prism.Events;

namespace FriendsOrganizer.UI.Startup
{
    /// <summary>
    /// Description of Bootstrapper.
    /// </summary>
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<FriendsOrganizerDbContext>().AsSelf();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<MessageDialogService>().As<IMessageDialogService>();
            
            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            builder.RegisterType<FriendDetailViewModel>()
                .Keyed<IDetailViewModel>(nameof(FriendDetailViewModel));

            builder.RegisterType<MeetingDetailViewModel>()
                .Keyed<IDetailViewModel>(nameof(MeetingDetailViewModel));

            builder.RegisterType<FriendRepository>().As<IFriendRepository>();
            builder.RegisterType<LookupDataService>().AsImplementedInterfaces();
            builder.RegisterType<MeetingRepository>().As<IMeetingRepository>();

            return builder.Build();
        }
    }
}