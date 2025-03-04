using CQRS.Core.Domain.Interfaces;
using CQRS.Core.Infrastructure.Interfaces;
using Post.Cmd.Infrastructure.Repositories;
using Post.Cmd.Infrastructure.Stores;

namespace Post;

public static partial class DependencyInjection
{
    public static IServiceCollection AddPostDependencies(this IServiceCollection services)
    {
        services.AddScoped<IEventStoreRepository, EventStoreRepository>();
        services.AddScoped<IEventStore, EventStore>();
        return services;
    }
}