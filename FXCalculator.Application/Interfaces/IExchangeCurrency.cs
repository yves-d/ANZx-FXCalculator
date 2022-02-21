using FXCalculator.Application.CurrencyExchangers;
using FXCalculator.Common.Models;

namespace FXCalculator.Application.Interfaces
{
    public interface IExchangeCurrency
    {
        decimal Exchange(decimal amount);

        ExchangeDaemon GetExchangeDaemon(CurrencySettlementMethod currencySettlementMethod);
    }
}
