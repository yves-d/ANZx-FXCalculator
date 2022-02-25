using FXCalculator.Application.CurrencyExchangers;
using FXCalculator.Common.Models;

namespace FXCalculator.Application.Interfaces
{
    public interface IExchangeCurrency
    {
        ExchangeInstrument GetExchangeInstrument(CurrencySettlementMethod currencySettlementMethod);
    }
}