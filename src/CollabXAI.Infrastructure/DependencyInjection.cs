using CollabXAI.Application.Interfaces;
using CollabXAI.Infrastructure.Data;
using CollabXAI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CollabXAI.Infrastructure;

// registers this layer's services (the database and the repository) with dependency injection,
// called once from Program.cs so startup stays tidy
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        // improvement: for sql server on azure use UseSqlServer and the SqlServer package, the rest stays the same
        services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));

        // scoped = a new repository for each web request
        services.AddScoped<ISmeRepository, SmeRepository>();
        return services;
    }
}
