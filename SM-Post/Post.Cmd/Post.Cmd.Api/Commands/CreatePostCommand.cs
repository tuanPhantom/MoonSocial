using CQRS.Core.Commands;

namespace Post.Cmd.Api.Commands;

/// <summary>
/// Represents a Command when a post is created
/// </summary>
public class CreatePostCommand : BaseCommand
{
    public string Author { get; set; }
    public string Message { get; set; }
}