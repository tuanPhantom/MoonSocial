using CQRS.Core.Commands;

namespace Post.Cmd.Api.Commands;

/// <summary>
/// Represents a Command when a comment is added
/// </summary>
public class AddCommentCommand : BaseCommand
{
    public string Comment { get; set; }
    public string Username { get; set; }
}