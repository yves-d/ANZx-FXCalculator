using FXCalculator.Application.Interfaces;
using FXCalculator.Common.Models;
using FXCalculator.Data.Interfaces;

namespace FXCalculator.Application.CurrencyExchangers
{
    public class DirectExchanger : IExchangeCurrency
    {
        ICurrencyRepository _currencyRepository;

        public DirectExchanger(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public decimal Exchange(decimal amount)
        {
            throw new NotImplementedException();
        }

        public ExchangeDaemon GetExchangeDaemon(CurrencySettlementMethod currencySettlementMethod)
        {
            var exchangeRate = _currencyRepository.GetCurrencyPairExchangeRate(currencySettlementMethod.Base, currencySettlementMethod.Term);
            var currencyPrecision = _currencyRepository.GetCurrencyDecimalPrecision(currencySettlementMethod.Term);
            return new ExchangeDaemon()
            {
                Rate = exchangeRate.Rate,
                Precision = currencyPrecision.DecimalPlaces
            };
        }
    }
}
