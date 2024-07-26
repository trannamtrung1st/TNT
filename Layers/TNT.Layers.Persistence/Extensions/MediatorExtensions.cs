using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;
using System.Threading.Tasks;
using TNT.Layers.Domain;
using TNT.Layers.Domain.Entities;
using TNT.Layers.Domain.Events;

namespace TNT.Layers.Persistence.Extensions
{
    public static class MediatorExtension
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator,
            DbContext ctx, DomainEventType eventType)
        {
            CheckDomainEvents(ctx, eventType, out var domainEvents, out var domainEntities);

            do
            {
                foreach (EntityEntry<DomainEntity> entry in domainEntities)
                {
                    entry.Entity.ClearDomainEvents(eventType);
                }

                foreach (var domainEvent in domainEvents)
                {
                    await mediator.Publish(domainEvent);
                }

                CheckDomainEvents(ctx, eventType, out domainEvents, out domainEntities);

            } while (domainEvents.Any());
        }

        private static void CheckDomainEvents(
            DbContext ctx, DomainEventType eventType,
            out DomainEvent[] domainEvents,
            out EntityEntry<DomainEntity>[] domainEntities)
        {
            domainEntities = ctx.ChangeTracker
                            .Entries<DomainEntity>()
                            .Where(x => x.Entity.GetDomainEvents(eventType)?.Any() == true)
                            .ToArray();

            domainEvents = domainEntities
                .SelectMany(x => x.Entity.GetDomainEvents(eventType))
                .ToArray();
        }
    }
}
