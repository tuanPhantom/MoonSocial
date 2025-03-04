using CQRS.Core.Handlers;
using Post.Cmd.Domain.Aggregates;
using Post.Cmd.Infrastructure.Handlers;

namespace Post.Cmd.Api.Commands;

/// <summary>
/// Represents the concrete colleague in the mediator pattern handling commands. <br/>
/// The CommandHandler class is the concrete colleague class that handles commands
/// by invoking the relevant PostAggregate and EventSourcingHandler methods.
/// </summary>
public class CommandHandler : ICommandHandler
{
    private readonly IEventSourcingHandler<PostAggregate> _eventSourcingHandler;
    private readonly ILogger<CommandHandler> _logger;

    public CommandHandler(IEventSourcingHandler<PostAggregate> eventSourcingHandler, ILogger<CommandHandler> logger)
    {
        _eventSourcingHandler = eventSourcingHandler;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task HandleAsync(CreatePostCommand command)
    {
        try
        {
            var aggregate = new PostAggregate(command.Id, command.Author, command.Message);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.StackTrace);
        }
    }

    /// <inheritdoc />
    public async Task HandleAsync(EditMessageCommand command)
    {
        try
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.EditMessage(command.Message);

            await _eventSourcingHandler.SaveAsync(aggregate);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.StackTrace);
        }
    }

    /// <inheritdoc />
    public async Task HandleAsync(LikePostCommand command)
    {
        try
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.LikePost();

            await _eventSourcingHandler.SaveAsync(aggregate);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.StackTrace);
        }
    }

    /// <inheritdoc />
    public async Task HandleAsync(AddCommentCommand command)
    {
        try
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.AddComment(command.Username, command.Comment);

            await _eventSourcingHandler.SaveAsync(aggregate);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.StackTrace);
        }
    }

    /// <inheritdoc />
    public async Task HandleAsync(EditCommentCommand command)
    {
        try
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.EditComment(command.CommentId, command.Comment, command.Username);

            await _eventSourcingHandler.SaveAsync(aggregate);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.StackTrace);
        }
    }

    /// <inheritdoc />
    public async Task HandleAsync(RemoveCommentCommand command)
    {
        try
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.RemoveComment(command.CommentId, command.Username);

            await _eventSourcingHandler.SaveAsync(aggregate);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.StackTrace);
        }
    }

    /// <inheritdoc />
    public async Task HandleAsync(RemovePostCommand command)
    {
        try
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.RemovePost(command.Username);

            await _eventSourcingHandler.SaveAsync(aggregate);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.StackTrace);
        }
    }
}