using FXCalculator.Common.Models;

namespace FXCalculator.Data.Interfaces
{
    public interface ICurrencyRepository
    {
        CurrencySettlementMethod GetCurrencySettlementMethod(string baseCurrency, string termCurrency);
        CurrencyPairExchangeRate GetCurrencyPairExchangeRate(string baseCurrency, string termCurrency);
        CurrencyDecimalPrecision GetCurrencyDecimalPrecision(string currency);
    }
}
