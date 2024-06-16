using Application.Abstractions;
using Coin_Api_Info.Extensions;
using Infrastructure.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection
            services, IConfiguration configuration)
    {
        services.AddHttpClient(configuration);
        services.AddScoped<ICoinApiService, CoinApiService>();
    }
}
