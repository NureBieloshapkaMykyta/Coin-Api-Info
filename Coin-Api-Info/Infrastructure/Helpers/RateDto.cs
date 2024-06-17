using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Helpers;

public class RateDto
{
    public DateTime time { get; set; }

    public string asset_id_base { get; set; }

    public decimal rate { get; set; }
}
