﻿using Prism.Events;

namespace FriendsOrganizer.UI.Event
{
    public class AfterCollectionSavedEvent : PubSubEvent<AfterCollectionSavedEventArgs>
    {

    }

    public class AfterCollectionSavedEventArgs
    {
        public string ViewModelName { get; set; }
    }
}