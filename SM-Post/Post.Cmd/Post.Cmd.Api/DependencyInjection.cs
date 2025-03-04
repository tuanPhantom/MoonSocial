using CQRS.Core.Domain.Interfaces;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure.Interfaces;
using Post.Cmd.Api.Commands;
using Post.Cmd.Domain.Aggregates;
using Post.Cmd.Infrastructure.Dispatchers;
using Post.Cmd.Infrastructure.Handlers;
using Post.Cmd.Infrastructure.Repositories;
using Post.Cmd.Infrastructure.Stores;

namespace Post;

public static partial class DependencyInjection
{
    public static IServiceCollection AddPostDependencies(this IServiceCollection services)
    {
        // Add services to the container.
        services.AddScoped<IEventStoreRepository, EventStoreRepository>();
        services.AddScoped<IEventStore, EventStore>();
        services.AddScoped<IEventSourcingHandler<PostAggregate>, EventSourcingHandler>();
        services.AddScoped<ICommandHandler, CommandHandler>();

        // Register command handler methods
        var commandHandler = services.BuildServiceProvider().GetRequiredService<ICommandHandler>();
        var dispatchers = new CommandDispatcher();
        dispatchers.RegisterHandler<CreatePostCommand>(commandHandler.HandleAsync);
        dispatchers.RegisterHandler<EditMessageCommand>(commandHandler.HandleAsync);
        dispatchers.RegisterHandler<LikePostCommand>(commandHandler.HandleAsync);
        dispatchers.RegisterHandler<AddCommentCommand>(commandHandler.HandleAsync);
        dispatchers.RegisterHandler<EditCommentCommand>(commandHandler.HandleAsync);
        dispatchers.RegisterHandler<RemoveCommentCommand>(commandHandler.HandleAsync);
        dispatchers.RegisterHandler<RemovePostCommand>(commandHandler.HandleAsync);
        services.AddSingleton<ICommandDispatcher>(_ => dispatchers);

        return services;
    }
}