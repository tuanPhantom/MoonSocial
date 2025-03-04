using CQRS.Core.Domain;
using Post.Common.Events;

namespace Post.Cmd.Domain.Aggregates;

public class PostAggregate : AggregateRoot
{
    private bool _active;
    private string _author;
    private Dictionary<Guid, Tuple<string, string>> _comments = new();

    public bool Active
    {
        get => _active;
        set => _active = value;
    }

    public PostAggregate()
    {
    }

    /// <summary>
    /// facilitates the handling of a command, that creates a new instance of the aggregate.
    /// </summary>
    /// <param name="id">the id of the aggregate</param>
    /// <param name="author">the user that created the post</param>
    /// <param name="message">the message of the post</param>
    public PostAggregate(Guid id, string author, string message)
    {
        RaiseEvent(new PostCreatedEvent()
        {
            Id = id,
            Author = author,
            Message = message,
            DatePosted = DateTime.Now
        });
    }

    /// <summary>
    /// This method is used to alter the state of the aggregate for <see cref="PostCreatedEvent"/>.
    /// </summary>
    /// <param name="event">The event that triggered this method</param>
    public void Apply(PostCreatedEvent @event)
    {
        _id = @event.Id;
        _active = true;
        _author = @event.Author;
    }

    /// <summary>
    /// This method for editing the message.
    /// </summary>
    /// <param name="message">the actual message to be edited</param>
    public void EditMessage(string message)
    {
        if (!_active)
        {
            throw new InvalidOperationException("You cannot edit the message of an inactive post!");
        }

        if (string.IsNullOrEmpty(message))
        {
            throw new InvalidOperationException($"The value of {nameof(message)} cannot be null or empty! Please provide a valid {nameof(message)}!");
        }

        RaiseEvent(new MessageUpdateEvent()
        {
            Id = _id,
            Message = message,
        });
    }

    /// <summary>
    /// This method is used to alter the state of the aggregate for <see cref="MessageUpdateEvent"/>.
    /// </summary>
    /// <param name="event">The event that triggered this method</param>
    public void Apply(MessageUpdateEvent @event)
    {
        _id = @event.Id;
    }

    /// <summary>
    /// This method for liking the post.
    /// </summary>
    public void LikePost()
    {
        if (!_active)
        {
            throw new InvalidOperationException("You cannot like the message of an inactive post!");
        }

        RaiseEvent(new PostLikedEvent()
        {
            Id = _id,
        });
    }

    /// <summary>
    /// This method is used to alter the state of the aggregate for <see cref="PostLikedEvent"/>.
    /// </summary>
    /// <param name="event">The event that triggered this method</param>
    public void Apply(PostLikedEvent @event)
    {
        _id = @event.Id;
    }

    /// <summary>
    /// This method is for adding new comment
    /// </summary>
    /// <param name="comment">the comment to be added</param>
    /// <param name="username">the username that created the comment</param> 
    public void AddComment(string comment, string username)
    {
        if (!_active)
        {
            throw new InvalidOperationException("You cannot add comment to an inactive post!");
        }

        if (string.IsNullOrEmpty(comment))
        {
            throw new InvalidOperationException($"The value of {nameof(comment)} cannot be null or empty! Please provide a valid {nameof(comment)}!");
        }

        RaiseEvent(new CommentAddedEvent()
        {
            Id = _id,
            CommentId = Guid.NewGuid(),
            Comment = comment,
            Username = username,
            CommentDate = DateTime.Now
        });
    }

    /// <summary>
    /// This method is used to alter the state of the aggregate for <see cref="CommentAddedEvent"/>.
    /// </summary>
    /// <param name="event">The event that triggered this method</param>
    public void Apply(CommentAddedEvent @event)
    {
        _id = @event.Id;
        _comments.Add(@event.CommentId, new Tuple<string, string>(@event.Comment, @event.Username));
    }

    /// <summary>
    /// This method is for editing a comment
    /// </summary>
    /// <param name="commentId">the Guid of the comment to be edited</param>
    /// <param name="username">the username that edited the comment</param>
    /// <param name="comment">the actual edited comment</param>
    public void EditComment(Guid commentId, string username, string comment)
    {
        if (!_active)
        {
            throw new InvalidOperationException("You cannot edit comment to an inactive post!");
        }

        if (!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
        {
            throw new InvalidOperationException("You are not allowed to edit a comment created by another user!");
        }

        RaiseEvent(new CommentUpdatedEvent()
        {
            Id = _id,
            CommentId = commentId,
            Comment = comment,
            Username = username,
            EditDate = DateTime.Now
        });
    }

    /// <summary>
    /// This method is used to alter the state of the aggregate for <see cref="CommentUpdatedEvent"/>.
    /// </summary>
    /// <param name="event">The event that triggered this method</param>
    public void Apply(CommentUpdatedEvent @event)
    {
        _id = @event.Id;
        _comments[@event.CommentId] = new Tuple<string, string>(@event.Comment, @event.Username);
    }

    /// <summary>
    /// This method is for remove a comment
    /// </summary>
    /// <param name="commentId">the Guid of the comment to be edited</param>
    /// <param name="username">the username that edited the comment</param>
    public void RemoveComment(Guid commentId, string username)
    {
        if (!_active)
        {
            throw new InvalidOperationException("You cannot remove comment to an inactive post!");
        }

        if (!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
        {
            throw new InvalidOperationException("You are not allowed to remove a comment created by another user!");
        }

        RaiseEvent(new CommentRemovedEvent()
        {
            Id = _id,
            CommentId = commentId,
        });
    }

    /// <summary>
    /// This method is used to alter the state of the aggregate for <see cref="CommentRemovedEvent"/>.
    /// </summary>
    /// <param name="event">The event that triggered this method</param>
    public void Apply(CommentRemovedEvent @event)
    {
        _id = @event.Id;
        _comments.Remove(@event.CommentId);
    }

    /// <summary>
    /// This method is for remove a post
    /// </summary>
    /// <param name="username">the username that edited the comment</param>
    public void RemovePost(string username)
    {
        if (!_active)
        {
            throw new InvalidOperationException("This post has already been removed!");
        }

        if (!username.Equals(_author, StringComparison.CurrentCultureIgnoreCase))
        {
            throw new InvalidOperationException("You are not allowed to remove a post created by another user!");
        }

        RaiseEvent(new PostRemovedEvent()
        {
            Id = _id
        });
    }

    /// <summary>
    /// This method is used to alter the state of the aggregate for <see cref="PostRemovedEvent"/>.
    /// </summary>
    /// <param name="event">The event that triggered this method</param>
    public void Apply(PostRemovedEvent @event)
    {
        _id = @event.Id;
        _active = false;
    }
}