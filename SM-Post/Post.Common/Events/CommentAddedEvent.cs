using CQRS.Core.Events;

namespace Post.Common.Events;

/// <summary>
/// This event will be raised when the Add Comment Command has been handled.
/// </summary>
public class CommentAddedEvent : BaseEvent
{
    public CommentAddedEvent() : base(nameof(CommentAddedEvent))
    {
    }

    public Guid CommentId { get; set; }
    public string Comment { get; set; }
    public string Username { get; set; }
    public DateTime CommentDate { get; set; }
}