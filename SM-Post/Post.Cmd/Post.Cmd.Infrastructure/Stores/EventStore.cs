using System.Transactions;
using CQRS.Core.Common.Exceptions;
using CQRS.Core.Domain.Interfaces;
using CQRS.Core.Events;
using CQRS.Core.Infrastructure.Interfaces;

namespace Post.Cmd.Infrastructure.Stores;

/// <summary>
/// The concrete event store for accessing the event store business logic.
/// This class acts like a Service of Infrastructure layer. <br/>
/// The Event Store is used on the write or command side of a CQRS and Event Sourcing based application,
/// and it is used to store data as a sequence of immutable events over time. <br/>
/// The Event Store uses <see cref="IEventStoreRepository"/> to write aggregates as events to the MongoDb (the writing database).
/// </summary>
public class EventStore : IEventStore
{
    private readonly IEventStoreRepository _eventStoreRepository;

    public EventStore(IEventStoreRepository eventStoreRepository)
    {
        _eventStoreRepository = eventStoreRepository;
    }

    /// <summary>
    /// This method versions events for given aggregate and for
    /// persisting new events to the event store via the event store repository.
    /// In other words, this method writes aggregates as events to the MongoDb (the writing database).
    /// </summary>
    /// <param name="aggregateId">identifier of the aggregate</param>
    /// <param name="events">a collection of uncommited events</param>
    /// <param name="expectedVersion">the current version of the event store</param>
    /// <returns>A Task represents versioning and persisting progress</returns>
    /// <remarks>
    /// This method will also contain logic needed to ensure that concurrent updates are appropriately handled
    /// using optimistic concurrency control.
    /// </remarks>
    public async Task SaveEventAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion)
    {
        var eventStream = await _eventStoreRepository.FindByAggregateId(aggregateId);

        // For the state of the Aggregate to be replayed/recreated correctly, it is important that the ordering of events is enforced by implementing event versioning.
        // So that the events are stored in the Event Store in the correct order or sequence.
        // Optimistic concurrency control is then used to ensure that only the expected event versions can be persisted in the event store.
        // This is especially important if two or more client requests are made concurrently to alter the state of the aggregate.
        if (expectedVersion != -1 && eventStream[^1].Version != expectedVersion)
        {
            throw new ConcurencyException();
        }

        var version = expectedVersion;

        using var session = await _eventStoreRepository.GetNewSessionAsync();
        try
        {
            session.StartTransaction();
            foreach (var @event in events)
            {
                version++;
                @event.Version = version;
                var eventType = @event.GetType().Name;
                var eventModel = new EventModel()
                {
                    TimeStamp = DateTime.Now,
                    AggregateIdentifier = aggregateId,
                    AggregateType = eventType,
                    Version = version,
                    EventType = eventType,
                    EventData = @event,
                };
                
                // write aggregates as events to the MongoDb (the writing database).
                await _eventStoreRepository.SaveAsync(eventModel); // event store needs to be inserted in a strict order (version).
            }
        }
        catch (Exception ex)
        {
            await session.AbortTransactionAsync();
            throw new TransactionException("Error while saving events", ex);
        }

        await session.CommitTransactionAsync();
    }

    /// <summary>
    /// This method is to retrieve all stored events
    /// from the event store, which is needed for replaying the latest state of the aggregate.
    /// </summary>
    /// <param name="aggregateId">identifier of the aggregate</param>
    /// <returns>A task represents the retrieving progress</returns>
    public async Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId)
    {
        var eventStream = await _eventStoreRepository.FindByAggregateId(aggregateId);

        if (eventStream == null || !eventStream.Any())
        {
            throw new AggregateNotFoundException("Incorrect aggregate provided!");
        }

        return eventStream.OrderBy(e => e.Version).Select(e => e.EventData).ToList();
    }
}