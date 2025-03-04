using CQRS.Core.Events;

namespace Post.Common.Events;

/// <summary>
/// This event will be raised when the Edit Message Command has been handled.
/// </summary>
public class MessageUpdateEvent : BaseEvent
{
    public MessageUpdateEvent() : base(nameof(MessageUpdateEvent))
    {
    }

    public string Message { get; set; }
}