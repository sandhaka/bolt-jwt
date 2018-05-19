using System.Linq;
using System.Threading.Tasks;
using BoltJwt.Domain.Model.Abstractions;
using BoltJwt.Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BoltJwt.Infrastructure.Extensions
{
    public static class MediatorExtension
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, IdentityContext ctx)
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<AggregateRoot>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            var entityEntries = domainEntities as EntityEntry<AggregateRoot>[] ?? domainEntities.ToArray();

            var domainEvents = entityEntries
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            entityEntries.ToList()
                .ForEach(entity => entity.Entity.DomainEvents.Clear());

            var tasks = domainEvents
                .Select(async (domainEvent) => {
                    await mediator.Publish(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}