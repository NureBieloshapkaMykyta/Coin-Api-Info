using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models;

public class CryptocurrencyPrice
{
    public Guid Id { get; set; }

    public decimal Price { get; set; }

    public DateTime LastUpdated { get; set; }

    public Guid CryptocurrencyId { get; set; }

    public virtual Cryptocurrency Cryptocurrency { get; set; }
}
