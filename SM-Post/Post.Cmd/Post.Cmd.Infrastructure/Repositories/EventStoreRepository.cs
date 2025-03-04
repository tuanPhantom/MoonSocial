using CQRS.Core.Domain.Interfaces;
using CQRS.Core.Events;
using CQRS.Core.Infrastructure.Persistence;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Post.Cmd.Infrastructure.Repositories;

/// <summary>
/// Represents the event store repository which will allow us to interact with our writing database or event store. <br/>
/// Now remember, in an event store, records are stored as a sequence of immutable events over time. <br/>
/// Therefore, we are not going to implement an update or a delete method. We will only have a method
/// that will allow us to persist new events to our event store as well as a method that allows us to
/// return events for a given aggregate. <br/>
/// In other words, this concrete class provides a method to write an event to MongoDb (the writing database).
/// </summary>
public class EventStoreRepository : IEventStoreRepository
{
    private static IOptions<MongoDbConfig> _config;
    private readonly IMongoCollection<EventModel> _eventStoreCollection;

    public EventStoreRepository(IOptions<MongoDbConfig> config)
    {
        _config ??= config;
        var mongoClient = new MongoClient(config.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(config.Value.Database);
        _eventStoreCollection = mongoDatabase.GetCollection<EventModel>(config.Value.Collection);
    }

    /// <inheritdoc />
    public async Task SaveAsync(EventModel @event)
    {
        await _eventStoreCollection.InsertOneAsync(@event).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<List<EventModel>> FindByAggregateId(Guid aggregateId)
    {
        return await _eventStoreCollection.Find(rec => rec.AggregateIdentifier == aggregateId).ToListAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public Task<IClientSessionHandle> GetNewSessionAsync()
    {
        var mongoClient = new MongoClient(_config.Value.ConnectionString);
        return mongoClient.StartSessionAsync();
    }
}