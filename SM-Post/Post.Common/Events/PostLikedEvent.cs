using CQRS.Core.Events;

namespace Post.Common.Events;

/// <summary>
/// This event will be raised when the Like Post Command has been handled.
/// </summary>
public class PostLikedEvent : BaseEvent
{
    public PostLikedEvent() : base(nameof(PostLikedEvent))
    {
    }
}