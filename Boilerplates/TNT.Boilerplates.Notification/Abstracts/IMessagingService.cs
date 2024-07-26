using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TNT.Boilerplates.Notification.Models;

namespace TNT.Boilerplates.Notification.Abstracts
{
    public interface IMessagingService
    {
        Task SendMessage(NotificationMessage message, CancellationToken cancellationToken = default);
        Task SendMessages(IEnumerable<NotificationMessage> messages, CancellationToken cancellationToken = default);
        Task SubcribeToTopic(string deviceToken, string topicName, CancellationToken cancellationToken = default);
        Task UnsubcribeFromTopic(string deviceToken, string topicName, CancellationToken cancellationToken = default);
    }
}
