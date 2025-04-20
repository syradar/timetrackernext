using Microsoft.Extensions.DependencyInjection;
using TimeTracking.Application.Repositories;

namespace TimeTracking.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<ITimeEntryRepository, TimeEntryRepository>();

        return services;
    }
}