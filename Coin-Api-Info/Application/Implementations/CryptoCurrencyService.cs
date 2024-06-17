using Application.Abstractions;
using Application.Helpers;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementations;

public class CryptoCurrencyService : ICryptoCurrencyService
{
    private readonly IRepository<Cryptocurrency> _cryptoCurrencyRepository;

    private readonly ICoinApiService _coinApiService;

    public CryptoCurrencyService(IRepository<Cryptocurrency> cryptoCurrencyRepository, ICoinApiService coinApiService)
    {
        _cryptoCurrencyRepository = cryptoCurrencyRepository;
        _coinApiService = coinApiService;
    }

    public async Task<Result<IEnumerable<Cryptocurrency>>> GetAllAsync(
        Expression<Func<Cryptocurrency, bool>>? filter = null)
    {
        return await _cryptoCurrencyRepository.GetAllAsync(filter);
    }

    public async Task<Result<IEnumerable<Cryptocurrency>>> GetPriceInformation(List<string> symbols)
    {
        if (symbols == null || !symbols.Any())
        {
            return Result.Failure<IEnumerable<Cryptocurrency>>("Symbols parameter is required.");
        }

        var results = new List<Cryptocurrency>();

        foreach (var symbol in symbols)
        {
            var getCryptocurrencyResult = await _cryptoCurrencyRepository.GetAllAsync(item => item.Symbol == symbol.ToUpper());
            if (!getCryptocurrencyResult.IsSuccessful)
            {
                return Result.Failure<IEnumerable<Cryptocurrency>>($"Cryptocurrency with symbol {symbol} not found." + getCryptocurrencyResult.Message);
            }

            var getWithPriceResult = await _coinApiService.GetRealTimePriceAsync(symbol);
            if (!getWithPriceResult.IsSuccessful)
            {
                return Result.Failure<IEnumerable<Cryptocurrency>>(getWithPriceResult.Message);
            }

            var updateCryptoCurrencyResult = await PerformPriceUpdate(getCryptocurrencyResult.Data.FirstOrDefault(), getWithPriceResult.Data);
            if (!updateCryptoCurrencyResult.IsSuccessful)
            {
                return Result.Failure<IEnumerable<Cryptocurrency>>("Failed to update price." + updateCryptoCurrencyResult.Message);
            }

            results.Add(getWithPriceResult.Data);
        }

        return Result.Success(results.AsEnumerable());
    }

    private async Task<Result<bool>> PerformPriceUpdate(Cryptocurrency? cryptocurrency, Cryptocurrency newCryptocurrency)
    {
        if (cryptocurrency != null)
        {
            cryptocurrency.Price = newCryptocurrency.Price;
            cryptocurrency.LastUpdated = newCryptocurrency.LastUpdated;
            var updateResult = await _cryptoCurrencyRepository.UpdateItemAsync(cryptocurrency);
            if (!updateResult.IsSuccessful)
            {
                return Result.Failure<bool>(updateResult.Message);
            }

            return Result.Success(true);
        }

        var addResult = await _cryptoCurrencyRepository.AddItemAsync(newCryptocurrency);
        if (!addResult.IsSuccessful)
        {
            return Result.Failure<bool>(addResult.Message);
        }

        return Result.Success(true);
    }
}
