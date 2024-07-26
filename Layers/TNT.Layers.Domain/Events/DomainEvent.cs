using MediatR;

namespace TNT.Layers.Domain.Events
{
    public class DomainEvent : INotification
    {
        public DomainEventType EventType { get; }

        protected DomainEvent(DomainEventType eventType)
        {
            EventType = eventType;
        }

        public static PrePersistedEvent<T> Pre<T>(T data) => new PrePersistedEvent<T>(DomainEventType.PrePersisted, data);
        public static PostPersistedEvent<T> Post<T>(T data) => new PostPersistedEvent<T>(DomainEventType.PostPersisted, data);
    }

    public class DomainEvent<T> : DomainEvent
    {
        public T Data { get; }

        protected DomainEvent(DomainEventType eventType, T data) : base(eventType)
        {
            Data = data;
        }
    }

    public class PrePersistedEvent<T> : DomainEvent<T>
    {
        public PrePersistedEvent(DomainEventType eventType, T data) : base(eventType, data)
        {
        }
    }

    public class PostPersistedEvent<T> : DomainEvent<T>
    {
        public PostPersistedEvent(DomainEventType eventType, T data) : base(eventType, data)
        {
        }
    }
}
