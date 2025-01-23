using CQRS.Core.Messages;

namespace CQRS.Core.Events;

/// <summary>
/// Events are objects that describe something that has occurred in the application.
/// A typical source of the event is the aggregate.
/// When something important happens in the aggregate, it will raise an event.
/// 
/// <br/> Events are used to confirm that an action has been successfully performed.
/// </summary>
public abstract class BaseEvent : Message
{
    protected BaseEvent(string type)
    {
        Type = type;
    }

    /// <summary>
    /// A version to replay the latest state of an aggregate
    /// </summary>
    public int Version { get; set; }

    /// <summary>
    /// Type acts as a discriminator property that we will
    /// use when we do polymorphic data binding when we serialize our event objects.
    /// </summary>
    public string Type { get; set; }
}