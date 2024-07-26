using System;
using System.Collections.Generic;

namespace TNT.Boilerplates.Notification.Models
{
    public class NotificationMessage : ICloneable
    {
        public NotificationInfo Notification { get; set; }
        public IReadOnlyDictionary<string, string> Data { get; set; }
        public string TargetIdentifier { get; set; }
        public string TargetTopic { get; set; }

        public NotificationMessage(NotificationInfo notification, IReadOnlyDictionary<string, string> data)
        {
            Notification = notification;
            Data = data;
        }

        public object Clone()
        {
            var notification = Notification?.Clone() as NotificationInfo;
            return new NotificationMessage(notification, data: new Dictionary<string, string>(Data))
            {
                TargetIdentifier = TargetIdentifier,
                TargetTopic = TargetTopic,
            };
        }
    }
}
