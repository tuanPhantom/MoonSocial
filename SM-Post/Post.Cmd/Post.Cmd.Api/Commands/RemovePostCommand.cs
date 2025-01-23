using CQRS.Core.Commands;

namespace Post.Cmd.Api.Commands;

/// <summary>
/// Represents a Command when a post is removed
/// </summary>
public class RemovePostCommand : BaseCommand
{
    public string Username { get; set; }
}