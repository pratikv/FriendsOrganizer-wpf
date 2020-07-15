using Prism.Events;

namespace FriendsOrganizer.UI.Event
{
    class AfterFriendSavedEvent: PubSubEvent<AfterFriendSavedEventArgs>
    {
    }

    public class AfterFriendSavedEventArgs
    {
        public int Id { get; set; }
        public string DisplayMember { get; set; }
    }
}