using CQRS.Core.Domain.Entities;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Post.Cmd.Domain.Aggregates;

namespace Post.Cmd.Infrastructure.Handlers;

/// <summary>
/// This class will provide a concrete implementation through which the command handler will obtain the latest state
/// of the aggregate and through which it will persist the uncommitted changes on the aggregate as events. <br/>
/// This concrete class uses <see cref="IEventStore"/> to write <see cref="AggregateRoot"/> as an event to the MongoDb (the writing database).
/// </summary>
public class EventSourcingHandler : IEventSourcingHandler<PostAggregate>
{
    private readonly IEventStore _eventStore;

    public EventSourcingHandler(IEventStore eventStore)
    {
        _eventStore = eventStore;
    }

    // <inheritdoc />
    public async Task SaveAsync(AggregateRoot aggregate)
    {
        await _eventStore.SaveEventAsync(aggregate.Id, aggregate.GetUncommittedChanges(), aggregate.Version);
        aggregate.MarkChangesAsCommitted();
    }

    // <inheritdoc />
    public async Task<PostAggregate> GetByIdAsync(Guid id)
    {
        var aggregate = new PostAggregate();
        var events = await _eventStore.GetEventsAsync(id);
        if (events == null || events.Count == 0)
        {
            return aggregate;
        }

        aggregate.ReplayEvent(events);
        aggregate.Version = events.Max(e => e.Version);
        return aggregate;
    }
}