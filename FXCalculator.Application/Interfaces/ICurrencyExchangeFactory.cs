using FXCalculator.Common.Models;

namespace FXCalculator.Application.Interfaces
{
    public interface ICurrencyExchangeFactory
    {
        IExchangeCurrency GetCurrencyExchanger(SettlementMethodEnum settlementMethodEnum);
    }
}
