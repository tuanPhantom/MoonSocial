using CQRS.Core.Commands;

namespace Post.Cmd.Api.Commands;

/// <summary>
/// Represents a Command when a message is edited
/// </summary>
public class EditMessageCommand : BaseCommand
{
    public string Message { get; set; }
}