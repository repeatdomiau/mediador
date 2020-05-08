using System;
using System.Collections.Generic;

namespace Mediador3
{
    internal class NotificationCacheItem
    {
        public Type Notification { get; private set; }
        public IEnumerable<Type> Listeners { get; set; }

        public NotificationCacheItem(Type notification, IEnumerable<Type> listeners)
        {
            this.Notification = notification;
            this.Listeners = listeners;
        }
    }

}
