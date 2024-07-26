namespace TNT.Boilerplates.Notification.Exceptions
{
    public class MessageSendException : NotificationException
    {
        public MessageSendException(object details) : base(details)
        {
        }
    }
}