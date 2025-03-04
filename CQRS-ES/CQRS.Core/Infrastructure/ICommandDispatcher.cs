using CQRS.Core.Commands;

namespace CQRS.Core.Infrastructure;

/// <summary>
/// An interface for the mediator pattern that used to dispatch the command among the registered handlers.
/// The ICommandDispatcher interface is the mediator that manages the distribution of messages (commands) among other objects.
/// </summary>
public interface ICommandDispatcher
{
    /// <summary>
    /// This method is for registering a command handler. 
    /// </summary>
    /// <param name="handler">the handler for <typeparamref name="T"/></param>
    /// <typeparam name="T">the actual type of <see cref="BaseCommand"/> to be handled</typeparam>
    void RegisterHandler<T>(Func<T, Task> handler) where T : BaseCommand;

    /// <summary>
    /// Send/Dispatch/Raise the command to the registered handlers.
    /// </summary>
    /// <param name="command">the command to be sent</param>
    /// <returns>A task represents the sending progress</returns>
    Task SendAsync(BaseCommand command);
}