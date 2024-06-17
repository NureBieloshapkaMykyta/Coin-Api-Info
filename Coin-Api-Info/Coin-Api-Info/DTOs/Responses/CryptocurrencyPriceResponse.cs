namespace Coin_Api_Info.DTOs.Responses;

public class CryptocurrencyPriceResponse
{
    public string Symbol { get; set; }

    public decimal Price { get; set; }

    public DateTime LastUpdated { get; set; }
}