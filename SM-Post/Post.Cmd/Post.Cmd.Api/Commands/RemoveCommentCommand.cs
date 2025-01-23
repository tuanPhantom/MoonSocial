using CQRS.Core.Commands;

namespace Post.Cmd.Api.Commands;

/// <summary>
/// Represents a Command when a comment is removed
/// </summary>
public class RemoveCommentCommand : BaseCommand
{
    public Guid CommentId { get; set; }
    public string Username { get; set; }
}