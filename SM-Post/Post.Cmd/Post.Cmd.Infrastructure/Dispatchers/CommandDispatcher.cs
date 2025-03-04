using CQRS.Core.Commands;
using CQRS.Core.Infrastructure;
using CQRS.Core.Infrastructure.Interfaces;

namespace Post.Cmd.Infrastructure.Dispatchers;

/// <summary>
/// The CommandDispatcher is the concrete mediator that is responsible for coordinating the colleague objects.
/// </summary>
public class CommandDispatcher : ICommandDispatcher
{
    private readonly Dictionary<Type, Func<BaseCommand, Task>> _handlers = new();

    /// <inheritdoc />
    public void RegisterHandler<T>(Func<T, Task> handler) where T : BaseCommand
    {
        if (_handlers.ContainsKey(typeof(T)))
        {
            throw new IndexOutOfRangeException($"Handler for type {typeof(T).FullName} already registered.");
        }

        // The delegate/lambda expression `cmd => handler((T)cmd)` is equivalent to the following:
        // Task SomeFunction(BaseCommand cmd)
        // {
        //      return handler((T)cmd);
        // }
        //
        // handler((T)cmd): Calls the handler function (which was passed as a parameter to RegisterHandler<T>) with the casted command.
        _handlers.Add(typeof(T), cmd => handler((T)cmd));
    }

    /// <summary>
    /// This is the method that will actually dispatch the command object to the registered command handler
    /// </summary>
    /// <inheritdoc />
    public async Task SendAsync(BaseCommand command)
    {
        if (_handlers.TryGetValue(command.GetType(), out var handler))
        {
            await handler(command);
        }
        else
        {
            throw new ArgumentNullException($"Handler for type {command.GetType().FullName} not registered.");
        }
    }
}