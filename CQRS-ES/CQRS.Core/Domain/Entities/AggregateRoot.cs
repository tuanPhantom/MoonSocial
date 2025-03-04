using CQRS.Core.Events;

namespace CQRS.Core.Domain.Entities;

/// <summary>
/// The aggregate is an entity or group of entities that is always kept in a consistent state,
/// and the aggregate root is the entity within the aggregate that is responsible for maintaining that state.
///
/// The aggregate can be viewed as the domain entity on the write or command side of CQRS in event
/// sourcing based application or service, similar to the domain entity that you find on the read or query
/// side.
///
/// In other words, in the write side, we store all state changes, and we save these changes in the form of events 
/// that are versioned.
/// 
/// The design of the aggregate should therefore allow you to be able to use these events to recreate or
/// replay the latest state of the aggregate so that you do not have to query the database for the latest
/// state.
/// Else, the hot separation of commands and queries would be in vain.
/// </summary>
public abstract class AggregateRoot
{
    protected Guid _id;

    /// <summary>
    /// Store all the uncommited (event) changes
    /// </summary>
    private readonly List<BaseEvent> _changes = new();

    public Guid Id
    {
        get => _id;
    }

    public int Version { get; set; } = -1;

    public IEnumerable<BaseEvent> GetUncommittedChanges() => _changes;

    public void MarkChangesAsCommitted() => _changes.Clear();

    /// <summary>
    /// Apply the action of the event, then add the provided <paramref name="event"/> to the <see cref="_changes"/>
    /// if the event is newly invoked.
    /// </summary>
    /// <param name="event">The provided event</param>
    /// <param name="isNew">Is the event newly invoked?</param>
    private void ApplyChange(BaseEvent @event, bool isNew)
    {
        var method = this.GetType().GetMethod("Apply", new Type[] { @event.GetType() });

        if (method == null) throw new ArgumentNullException(nameof(method), $"The applied method was not found in the aggregate for {@event.GetType().Name}!.");

        method.Invoke(this, new object[] { @event });

        if (isNew)
        {
            _changes.Add(@event);
        }
    }

    protected void RaiseEvent(BaseEvent @event)
    {
        ApplyChange(@event, true);
    }

    /// <summary>
    /// recreate or replay the latest state of the aggregate in the provided <paramref name="events"/>
    /// </summary>
    /// <param name="events">the collection of events to be recreated or replayed the latest state of the aggregate</param>
    public void ReplayEvent(IEnumerable<BaseEvent> events)
    {
        foreach (var @event in events)
        {
            ApplyChange(@event, false);
        }
    }
}