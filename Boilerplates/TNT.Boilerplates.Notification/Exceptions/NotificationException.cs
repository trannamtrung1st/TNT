using System;

namespace TNT.Boilerplates.Notification.Exceptions
{
    public abstract class NotificationException : Exception
    {
        public NotificationException(object details)
        {
            Details = details;
        }

        public virtual object Details { get; }
    }
}