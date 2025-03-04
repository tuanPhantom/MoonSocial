using CQRS.Core.Domain.Interfaces;
using Post.Cmd.Infrastructure.Repositories;

namespace Post;

public static partial class DependencyInjection
{
    public static IServiceCollection AddPostDependencies(this IServiceCollection services)
    {
        services.AddScoped<IEventStoreRepository, EventStoreRepository>();
        return services;
    }
}