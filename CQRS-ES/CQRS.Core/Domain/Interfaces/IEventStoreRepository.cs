using CQRS.Core.Events;
using MongoDB.Driver;

namespace CQRS.Core.Domain.Interfaces;

/// <summary>
/// Represents the event store repository which will allow us to interact with our writing database or event store. <br/>
/// Now remember, in an event store, records are stored as a sequence of immutable events over time. <br/>
/// Therefore, we are not going to implement an update or a delete method. We will only have a method
/// that will allow us to persist new events to our event store as well as a method that allows us to
/// return events for a given aggregate. <br/>
/// In other words, this interface provides a method to write an event to MongoDb (the writing database).
/// </summary>
public interface IEventStoreRepository : IRepository
{
    /// <summary>
    /// This method will be used to persist a new event to the event store.
    /// In other words, this method writes event to MongoDb (the writing database)
    /// </summary>
    /// <param name="event">A new record that is going to be in the event store</param>
    /// <returns>A Task represents the persisting progress</returns>
    Task SaveAsync(EventModel @event);

    /// <summary>
    /// This method will be used for retrieving all the events from the event store that
    /// matches the specific <paramref name="aggregateId"/> 
    /// </summary>
    /// <param name="aggregateId">The given aggregate id of the queried events</param>
    /// <returns>A Task represents the retrieving progress</returns>
    Task<List<EventModel>> FindByAggregateId(Guid aggregateId);

    /// <summary>
    /// Create a new session using MongoDb client
    /// </summary>
    /// <returns>a new session in the type of <see cref="IClientSessionHandle"/></returns>
    Task<IClientSessionHandle> GetNewSessionAsync();
}