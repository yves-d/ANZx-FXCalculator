using FXCalculator.Application.Interfaces;
using FXCalculator.Common.Models;
using FXCalculator.Data.Interfaces;

namespace FXCalculator.Application.CurrencyExchangers
{
    public class InvertedExchanger : IExchangeCurrency
    {
        ICurrencyRepository _currencyRepository;

        public InvertedExchanger(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public decimal Exchange(decimal amount)
        {
            throw new NotImplementedException();
        }

        public ExchangeDaemon GetExchangeDaemon(CurrencySettlementMethod currencySettlementMethod)
        {
            var exchangeRate = _currencyRepository.GetCurrencyPairExchangeRate(currencySettlementMethod.Term, currencySettlementMethod.Base);
            var currencyPrecision = _currencyRepository.GetCurrencyDecimalPrecision(currencySettlementMethod.Base);
            return new ExchangeDaemon()
            {
                Rate = 1 / exchangeRate.Rate,
                Precision = currencyPrecision.DecimalPlaces
            };
        }
    }
}
