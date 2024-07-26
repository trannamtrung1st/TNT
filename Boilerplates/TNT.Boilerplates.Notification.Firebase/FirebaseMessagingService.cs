using FirebaseAdmin.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TNT.Boilerplates.Notification.Abstracts;
using TNT.Boilerplates.Notification.Exceptions;
using TNT.Boilerplates.Notification.Firebase.Abstracts;
using TNT.Boilerplates.Notification.Models;
using FirebaseNotification = FirebaseAdmin.Messaging.Notification;

namespace TNT.Boilerplates.Notification.Firebase
{
    public class FirebaseMessagingService : IMessagingService
    {
        private readonly IFirebaseService _firebaseService;

        public FirebaseMessagingService(IFirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }

        public async Task SendMessage(NotificationMessage message, CancellationToken cancellationToken = default)
        {
            try
            {
                Message fbMess = ToMessage(message);

                string messageId = await _firebaseService.Messaging.SendAsync(fbMess, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new MessageSendException(details: ex);
            }
        }

        public async Task SendMessages(IEnumerable<NotificationMessage> messages, CancellationToken cancellationToken = default)
        {
            IEnumerable<Message> fbMessages = messages.Select(ToMessage).ToArray();

            BatchResponse response = await _firebaseService.Messaging.SendEachAsync(fbMessages, cancellationToken);

            if (response.FailureCount > 0)
                throw new MessageSendException(details: response);
        }

        public async Task SubcribeToTopic(string token, string topicName, CancellationToken cancellationToken = default)
        {
            TopicManagementResponse response = await _firebaseService.Messaging.SubscribeToTopicAsync(new[]
            {
                token
            }, topicName);

            if (response.FailureCount > 0)
                throw new TopicSubscriptionException(response);
        }

        public async Task UnsubcribeFromTopic(string token, string topicName, CancellationToken cancellationToken = default)
        {
            TopicManagementResponse response = await _firebaseService.Messaging.UnsubscribeFromTopicAsync(new[]
            {
                token
            }, topicName);

            if (response.FailureCount > 0)
                throw new TopicSubscriptionException(response);
        }

        private static Message ToMessage(NotificationMessage message)
        {
            if (message.Notification == null && message.Data?.Any() != true)
                throw new ArgumentException("A Firebase message must have notification or data");

            if (string.IsNullOrWhiteSpace(message.TargetTopic)
                && string.IsNullOrWhiteSpace(message.TargetIdentifier))
                throw new ArgumentException("A Firebase message must have target token or topic");

            return new Message
            {
                Notification = message.Notification != null ? ToNotification(message.Notification) : null,
                Data = message.Data,
                Token = message.TargetIdentifier,
                Topic = message.TargetTopic
            };
        }

        private static FirebaseNotification ToNotification(NotificationInfo info)
        {
            if (string.IsNullOrWhiteSpace(info.Title)) throw new ArgumentException(nameof(info.Title));
            if (string.IsNullOrWhiteSpace(info.Body)) throw new ArgumentException(nameof(info.Body));

            return new FirebaseNotification
            {
                Body = info.Body,
                ImageUrl = info.ImageUrl,
                Title = info.Title,
            };
        }
    }
}
