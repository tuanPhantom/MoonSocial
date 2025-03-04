using CQRS.Core.Events;

namespace Post.Common.Events;

/// <summary>
/// This event will be raised when the Create Post Command has been handled.
/// </summary>
public class PostCreatedEvent : BaseEvent
{
    public PostCreatedEvent() : base(nameof(PostCreatedEvent))
    {
    }

    public string Author { get; set; }
    public string Message { get; set; }
    public DateTime DatePosted { get; set; }
}