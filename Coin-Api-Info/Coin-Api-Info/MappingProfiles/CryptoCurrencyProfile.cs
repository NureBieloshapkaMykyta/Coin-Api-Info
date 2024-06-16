using AutoMapper;
using Coin_Api_Info.DTOs.Responses;
using Domain.Models;

namespace Coin_Api_Info.MappingProfiles;

public class CryptoCurrencyProfile : Profile
{
    public CryptoCurrencyProfile()
    {
        CreateMap<Cryptocurrency, CryptocurrencyResponse>();
    }
}
