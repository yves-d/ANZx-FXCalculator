using FXCalculator.Application.Interfaces;
using FXCalculator.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXCalculator.Application.CurrencyExchangers
{
    public abstract class CurrencyExchangeBase : IExchangeCurrency
    {
        public decimal Exchange(decimal amount)
        {
            throw new NotImplementedException();
        }

        public ExchangeInstrument GetExchangeInstrument(CurrencySettlementMethod currencySettlementMethod)
        {
            throw new NotImplementedException();
        }
    }
}
