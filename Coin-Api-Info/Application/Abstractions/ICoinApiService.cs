using Application.Helpers;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions;

public interface ICoinApiService
{
    Task<Result<Cryptocurrency>> GetRealTimePriceAsync(string symbol);
}
