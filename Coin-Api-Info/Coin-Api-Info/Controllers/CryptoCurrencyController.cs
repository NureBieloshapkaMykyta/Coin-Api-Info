using Application.Abstractions;
using AutoMapper;
using Coin_Api_Info.DTOs.Requests;
using Coin_Api_Info.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Coin_Api_Info.Controllers;

[Route("api/[controller]")]
public class CryptocurrencyController : Controller
{
    private readonly IMapper _mapper;

    private readonly ICryptoCurrencyService _cryptoCurrencyService;

    public CryptocurrencyController(ICryptoCurrencyService cryptoCurrencyService, IMapper mapper)
    {
        _cryptoCurrencyService = cryptoCurrencyService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetSupportedCryptocurrencies()
    {
        var getCryptocurrenciesResult = await _cryptoCurrencyService.GetAllAsync();
        if (!getCryptocurrenciesResult.IsSuccessful)
        {
            return BadRequest(getCryptocurrenciesResult.Message);
        }

        return Ok(getCryptocurrenciesResult.Data.Select(_mapper.Map<CryptocurrencyResponse>));
    }

    [HttpGet("prices")]
    public async Task<IActionResult> GetPriceInformation([FromQuery] GetCryptocurrenciesBySymbolsRequest request)
    {
        var getPriceResult = await _cryptoCurrencyService.GetPriceInformation(request.Symbols);
        if (!getPriceResult.IsSuccessful)
        {
            return BadRequest(getPriceResult.Message);
        }

        return Ok(getPriceResult.Data.Select(_mapper.Map<CryptocurrencyPriceResponse>));
    }
}
