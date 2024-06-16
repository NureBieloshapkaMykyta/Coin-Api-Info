namespace Coin_Api_Info.Extensions;

public static class HttpClientExtension
{
    public static void AddHttpClient(this IServiceCollection services, IConfiguration configuration) 
    {
        var coinApiSection = configuration.GetSection("CoinApi");
        services.AddHttpClient(coinApiSection.GetValue<string>("Client"), client => 
        {
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("X-CoinAPI-Key", coinApiSection.GetValue<string>("ApiKey"));
        });
    }
}
