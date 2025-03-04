using CQRS.Core.Events;

namespace CQRS.Core.Infrastructure.Interfaces;

/// <summary>
/// an interface abstraction for accessing the event store business logic.
/// </summary>
public interface IEventStore
{
    Task SaveEventAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion);
    Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId);
}