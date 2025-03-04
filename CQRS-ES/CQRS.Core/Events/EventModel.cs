using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CQRS.Core.Events;

/// <summary>
/// The purpose of the event model is to represent the schema of the event store, and each instance of the
/// event model will represent a record in the event store or more accurately.
/// <br/>
/// Since we are using MongoDB for our event store, it is better to say that each instance of the event
/// model will represent a document in the event store collection, and each document again will represent
/// an event that is versioned that can alter the state of the aggregate.
/// </summary>
public class EventModel
{
    /// <summary>
    /// MongoDB object's id 
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    /// <summary>
    /// The time stamp when an event is persisted
    /// </summary>
    public DateTime TimeStamp { get; set; }

    /// <summary>
    /// Identifier of the aggregate that an event is related to
    /// </summary>
    public Guid AggregateIdentifier { get; set; }

    /// <summary>
    /// Type of the aggregate that an event is related to
    /// </summary>
    public string AggregateType { get; set; }

    /// <summary>
    /// Version of this event model
    /// </summary>
    public int Version { get; set; }

    /// <summary>
    /// Type of the event
    /// </summary>
    public string EventType { get; set; }

    /// <summary>
    /// The actual Event
    /// </summary>
    public BaseEvent EventData { get; set; }
}