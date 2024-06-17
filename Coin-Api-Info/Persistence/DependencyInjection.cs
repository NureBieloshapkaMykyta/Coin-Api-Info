using Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.DataAccess;

namespace Persistence;

public static class DependencyInjection
{
    public static void AddPersistence(this IServiceCollection
            services, IConfiguration configuration, string connectionStringSection)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options
                .UseLazyLoadingProxies()
                .UseNpgsql(configuration.GetConnectionString(connectionStringSection));
        });

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    }
}
