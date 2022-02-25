using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXCalculator.Console.Models
{
    public class CurrencyExchangeRequest
    {
        public string Base { get; init; }
        public string Term { get; init; }
        public decimal Amount { get; init; }

    }
}
