using Application.Abstractions;
using Application.Helpers;
using Domain.Models;
using Infrastructure.Helpers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Implementations;

public class CoinApiService : ICoinApiService
{
    private readonly HttpClient _httpClient;

    public CoinApiService(IHttpClientFactory factory, IConfiguration configuration)
    {
        _httpClient = factory.CreateClient(configuration.GetSection("CoinApi").GetValue<string>("Client"));
    }

    public async Task<Result<Cryptocurrency>> GetRealTimePriceAsync(string symbol)
    {
        try
        {
            var response = await _httpClient.GetAsync($"https://rest.coinapi.io/v1/exchangerate/{symbol}/USD");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);
            var rate = json.ToObject<RateDto>();

            return Result.Success(new Cryptocurrency() 
            {
                Symbol = rate.asset_id_base,
                LastUpdated = rate.time,
                Price = rate.rate
            });
        }
        catch (Exception e) when (
            e is InvalidOperationException ||
            e is TaskCanceledException ||
            e is UriFormatException)
        {
            throw;
        }
        catch (HttpRequestException e)
        {
            return Result.Failure<Cryptocurrency>("Invalid data provided to access third-party API" + e.InnerException);
        }
    }
}