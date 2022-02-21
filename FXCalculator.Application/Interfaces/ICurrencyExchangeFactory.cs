using FXCalculator.Common.Models;

namespace FXCalculator.Application.Interfaces
{
    public interface ICurrencyExchangeFactory
    {
        //IExchangeCurrency GetCurrencyExchanger(string baseCurrency, string termsCurrency, CurrencySettlementMethod currencySettlementMethod);
        IExchangeCurrency GetCurrencyExchanger(SettlementMethodEnum settlementMethodEnum);
    }
}
