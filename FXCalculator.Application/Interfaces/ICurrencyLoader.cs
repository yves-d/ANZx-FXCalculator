using FXCalculator.Common.Models;

namespace FXCalculator.Application.Interfaces
{
    public interface ICurrencyLoader
    {
        CurrencySettlementMethod GetCurrencySettlementMethod(string baseCurrency, string termCurrency);
        CurrencyPairExchangeRate GetCurrencyPairExchangeRate(string baseCurrency, string termCurrency);
        CurrencyDecimalPrecision GetCurrencyDecimalPrecision(string currency);
    }
}