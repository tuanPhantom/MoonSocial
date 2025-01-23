using CQRS.Core.Events;

namespace Post.Common.Events;

/// <summary>
/// This event will be raised when the Edit Comment Command has been handled.
/// </summary>
public class CommentUpdatedEvent : BaseEvent
{
    public CommentUpdatedEvent() : base(nameof(CommentUpdatedEvent))
    {
    }

    public Guid? CommentId { get; set; }
    public string Comment { get; set; }
    public string Username { get; set; }
    public DateTime EditDate { get; set; }
}