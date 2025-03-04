using CQRS.Core.Events;

namespace Post.Common.Events;

/// <summary>
/// This event will be raised when the Remove Post Command has been handled.
/// </summary>
public class PostRemovedEvent : BaseEvent
{
    public PostRemovedEvent() : base(nameof(PostRemovedEvent))
    {
    }
}