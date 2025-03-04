namespace Post.Cmd.Api.Commands;

/// <summary>
/// Represents the abstract colleague in the mediator pattern handling commands.
/// </summary>
public interface ICommandHandler
{
    /// <summary>
    /// Handle the event for the type <see cref="CreatePostCommand"/>
    /// </summary>
    /// <param name="command">The concrete command that triggered the event</param>
    /// <returns>A task represents the handling progress</returns>
    Task HandleAsync(CreatePostCommand command);

    /// <summary>
    /// Handle the event for the type <see cref="EditMessageCommand"/>
    /// </summary>
    /// <param name="command">The concrete command that triggered the event</param>
    /// <returns>A task represents the handling progress</returns>
    Task HandleAsync(EditMessageCommand command);

    /// <summary>
    /// Handle the event for the type <see cref="LikePostCommand"/>
    /// </summary>
    /// <param name="command">The concrete command that triggered the event</param>
    /// <returns>A task represents the handling progress</returns>
    Task HandleAsync(LikePostCommand command);

    /// <summary>
    /// Handle the event for the type <see cref="AddCommentCommand"/>
    /// </summary>
    /// <param name="command">The concrete command that triggered the event</param>
    /// <returns>A task represents the handling progress</returns>
    Task HandleAsync(AddCommentCommand command);

    /// <summary>
    /// Handle the event for the type <see cref="EditCommentCommand"/>
    /// </summary>
    /// <param name="command">The concrete command that triggered the event</param>
    /// <returns>A task represents the handling progress</returns>
    Task HandleAsync(EditCommentCommand command);

    /// <summary>
    /// Handle the event for the type <see cref="RemoveCommentCommand"/>
    /// </summary>
    /// <param name="command">The concrete command that triggered the event</param>
    /// <returns>A task represents the handling progress</returns>
    Task HandleAsync(RemoveCommentCommand command);

    /// <summary>
    /// Handle the event for the type <see cref="RemovePostCommand"/>
    /// </summary>
    /// <param name="command">The concrete command that triggered the event</param>
    /// <returns>A task represents the handling progress</returns>
    Task HandleAsync(RemovePostCommand command);
}