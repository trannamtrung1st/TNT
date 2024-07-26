using System;
using System.Collections.Generic;
using TNT.Layers.Domain.Events;

namespace TNT.Layers.Domain.Entities
{
    public abstract class DomainEntity
    {
        private List<DomainEvent> _preDomainEvents;
        private ISet<DomainEvent> _preDomainEventsSet;

        private List<DomainEvent> _postDomainEvents;
        private ISet<DomainEvent> _postDomainEventsSet;

        public IReadOnlyList<DomainEvent> GetDomainEvents(DomainEventType eventType)
        {
            GetEvents(eventType, out List<DomainEvent> events, out _);

            return events;
        }


        protected void AddPreEvent<T>(T data) => AddDomainEvent(DomainEvent.Pre(data));
        protected void AddPostEvent<T>(T data) => AddDomainEvent(DomainEvent.Post(data));
        protected void AddPipelineEvent<T>(T data)
        {
            AddDomainEvent(DomainEvent.Pre(data));
            AddDomainEvent(DomainEvent.Post(data));
        }

        private void AddDomainEvent(DomainEvent eventItem)
        {
            if (eventItem == null) throw new ArgumentNullException(nameof(eventItem));

            InitEvents();

            GetEvents(eventItem.EventType, out List<DomainEvent> events, out ISet<DomainEvent> eventSet);

            if (eventSet.Add(eventItem))
            {
                events.Add(eventItem);
            }
        }

        public void RemoveDomainEvent(DomainEvent eventItem)
        {
            if (eventItem == null) throw new ArgumentNullException(nameof(eventItem));

            GetEvents(eventItem.EventType, out List<DomainEvent> events, out ISet<DomainEvent> eventSet);

            if (eventSet?.Remove(eventItem) == true)
            {
                events?.Remove(eventItem);
            }
        }

        public void ClearDomainEvents(DomainEventType eventType)
        {
            GetEvents(eventType, out List<DomainEvent> events, out ISet<DomainEvent> eventSet);

            events?.Clear();
            eventSet?.Clear();
        }

        private void InitEvents()
        {
            _preDomainEvents ??= new List<DomainEvent>();
            _preDomainEventsSet ??= new HashSet<DomainEvent>();
            _postDomainEvents ??= new List<DomainEvent>();
            _postDomainEventsSet ??= new HashSet<DomainEvent>();
        }

        private void GetEvents(DomainEventType eventType,
            out List<DomainEvent> events, out ISet<DomainEvent> eventSet)
        {
            events = eventType == DomainEventType.PrePersisted
                ? _preDomainEvents : _postDomainEvents;
            eventSet = eventType == DomainEventType.PrePersisted
                ? _preDomainEventsSet : _postDomainEventsSet;
        }
    }
}
