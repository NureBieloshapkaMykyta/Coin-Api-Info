namespace Coin_Api_Info.DTOs.Responses;

public class CryptocurrencyResponse
{
    public class Cryptocurrency
    {
        public Guid Id { get; set; }

        public string Symbol { get; set; }
    }
}