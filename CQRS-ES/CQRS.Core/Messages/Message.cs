namespace CQRS.Core.Messages;

/// <summary>
/// A message is a generic term for any data structure used for communication between different system parts.
/// In a CQRS architecture, "message" is a broad term that encompasses Commands, Queries, and Events
/// as all are types of messages used to facilitate communication between components of the system.
/// </summary>
public abstract class Message
{
    public Guid Id { get; set; }
}