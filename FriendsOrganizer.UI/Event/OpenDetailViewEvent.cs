using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;

namespace FriendsOrganizer.UI.Event
{
    public class OpenDetailViewEvent : PubSubEvent<OpenDetailViewEventArgs>
    {
    }

    public class OpenDetailViewEventArgs
    {
        public int Id { get; set; }
        public string ViewModelName { get; set; }
    }
}
