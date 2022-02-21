using FXCalculator.Application.Interfaces;
using FXCalculator.Common.Models;
using FXCalculator.Data.Interfaces;

namespace FXCalculator.Application.CurrencyExchangers
{
    public class OneToOneExchanger : IExchangeCurrency
    {
        ICurrencyRepository _currencyRepository;

        public OneToOneExchanger(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public decimal Exchange(decimal amount)
        {
            throw new NotImplementedException();
        }

        public ExchangeDaemon GetExchangeDaemon(CurrencySettlementMethod currencySettlementMethod)
        {
            var currencyPrecision = _currencyRepository.GetCurrencyDecimalPrecision(currencySettlementMethod.Base);
            return new ExchangeDaemon()
            {
                Rate = 1.00m,
                Precision = currencyPrecision.DecimalPlaces
            };
        }
    }
}
