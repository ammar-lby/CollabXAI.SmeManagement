using CollabXAI.Application.Interfaces;
using CollabXAI.Infrastructure.Data;
using CollabXAI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CollabXAI.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));

        // scoped = a new repository for each web request
        services.AddScoped<ISmeRepository, SmeRepository>();
        return services;
    }
}
