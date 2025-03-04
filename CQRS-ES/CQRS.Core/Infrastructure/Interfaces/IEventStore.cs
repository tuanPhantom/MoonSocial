using CQRS.Core.Events;

namespace CQRS.Core.Infrastructure.Interfaces;

/// <summary>
/// An interface abstraction for accessing the event store business logic.
/// The Event Store is used on the write or command side of a CQRS and Event Sourcing based application,
/// and it is used to store data as a sequence of immutable events over time. <br/>
/// The Event Store writes aggregates as events to the MongoDb (the writing database).
/// </summary>
public interface IEventStore
{
    Task SaveEventAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion);
    Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId);
}