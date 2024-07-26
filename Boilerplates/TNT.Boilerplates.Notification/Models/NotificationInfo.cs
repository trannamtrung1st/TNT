using System;

namespace TNT.Boilerplates.Notification.Models
{
    public class NotificationInfo : ICloneable
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string ImageUrl { get; set; }

        public NotificationInfo(string title, string body)
        {
            Title = title;
            Body = body;
        }

        public object Clone() => new NotificationInfo(Title, Body)
        {
            ImageUrl = ImageUrl
        };
    }
}
