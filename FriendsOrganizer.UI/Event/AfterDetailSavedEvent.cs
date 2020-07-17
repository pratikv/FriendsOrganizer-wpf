using Prism.Events;

namespace FriendsOrganizer.UI.Event
{
    class AfterDetailSavedEvent: PubSubEvent<AfterSavedEventArgs>
    {
    }

    public class AfterSavedEventArgs
    {
        public int Id { get; set; }
        public string DisplayMember { get; set; }
        public string ViewModelName { get; set; }
    }
}