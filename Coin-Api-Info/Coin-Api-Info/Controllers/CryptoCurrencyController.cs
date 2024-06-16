using Application.Abstractions;
using AutoMapper;
using Coin_Api_Info.DTOs.Responses;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coin_Api_Info.Controllers;

[Route("[controller]")]
public class CryptoCurrencyController : Controller
{
    private readonly ICoinApiService _coinApiService;

    private readonly IRepository<Cryptocurrency> _cryptoCurrencyRepository;

    private readonly IMapper _mapper;

    public CryptoCurrencyController(ICoinApiService coinApiService, IRepository<Cryptocurrency> cryptoCurrencyRepository, IMapper mapper)
    {
        _coinApiService = coinApiService;
        _cryptoCurrencyRepository = cryptoCurrencyRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetSupportedCryptocurrencies()
    {
        var getCryptocurrenciesResult = await _cryptoCurrencyRepository.GetAllAsync();
        if (!getCryptocurrenciesResult.IsSuccessful)
        {
            return BadRequest(getCryptocurrenciesResult.Message);
        }

        return Ok(getCryptocurrenciesResult.Data.Select(_mapper.Map<CryptocurrencyResponse>));
    }

    //[HttpGet("prices")]
    //public async Task<IActionResult> GetPriceInformation([FromQuery] List<string> symbols)
    //{
    //    if (symbols == null || !symbols.Any())
    //    {
    //        return BadRequest("Symbols parameter is required.");
    //    }

    //    var results = new List<object>();

    //    foreach (var symbol in symbols)
    //    {
    //        var cryptocurrency = await _context.Cryptocurrencies.FirstOrDefaultAsync(c => c.Symbol == symbol.ToUpper());
    //        if (cryptocurrency == null)
    //        {
    //            return NotFound($"Cryptocurrency with symbol {symbol} not found.");
    //        }

    //        var price = await _coinApiService.GetRealTimePriceAsync(symbol);
    //        var cryptocurrencyPrice = new CryptocurrencyPrice
    //        {
    //            CryptocurrencyId = cryptocurrency.Id,
    //            Price = price,
    //            LastUpdated = DateTime.UtcNow
    //        };

    //        _context.CryptocurrencyPrices.Add(cryptocurrencyPrice);
    //        await _context.SaveChangesAsync();

    //        results.Add(new
    //        {
    //            cryptocurrency.Symbol,
    //            cryptocurrency.Name,
    //            cryptocurrencyPrice.Price,
    //            cryptocurrencyPrice.LastUpdated
    //        });
    //    }

    //    return Ok(results);
    //}
}
