using Application.Helpers;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions;

public interface ICryptoCurrencyService
{
    Task<Result<IEnumerable<Cryptocurrency>>> GetAllAsync(
        Expression<Func<Cryptocurrency, bool>>? filter = null);

    Task<Result<IEnumerable<Cryptocurrency>>> GetPriceInformation(List<string> symbols);
}
