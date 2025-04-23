using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TimeTracking.Application.Database;
using TimeTracking.Application.Repositories;
using TimeTracking.Application.Services;

namespace TimeTracking.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<ITimeEntryRepository, TimeEntryRepository>();
        services.AddSingleton<ITimeEntryService, TimeEntryService>();
        services.AddSingleton<IClientRepository, ClientRepository>();
        services.AddSingleton<IClientService, ClientService>();
        services.AddSingleton<IBookmarkRepository, BookmarkRepository>();
        services.AddSingleton<IBookmarkService, BookmarkService>();

        services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Singleton);

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(_ => new NpgsqlDbConnectionFactory(connectionString));
        services.AddSingleton<DbInitializer>();
        return services;
    }
}