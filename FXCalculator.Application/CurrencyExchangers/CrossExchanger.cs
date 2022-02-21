using FXCalculator.Application.Interfaces;
using FXCalculator.Common.Models;
using FXCalculator.Data.Interfaces;

namespace FXCalculator.Application.CurrencyExchangers
{
    public class CrossExchanger : IExchangeCurrency
    {
        ICurrencyRepository _currencyRepository;

        public CrossExchanger(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public decimal Exchange(decimal amount)
        {
            throw new NotImplementedException();
        }

        public ExchangeDaemon GetExchangeDaemon(CurrencySettlementMethod currencySettlementMethod)
        {
            var exchangeRateBaseToCross = _currencyRepository.GetCurrencyPairExchangeRate(currencySettlementMethod.Base, currencySettlementMethod.SettlementCurrency);
            var exchangeRateCrossToTerm = _currencyRepository.GetCurrencyPairExchangeRate(currencySettlementMethod.SettlementCurrency, currencySettlementMethod.Term);
            
            
            var currencyPrecision = _currencyRepository.GetCurrencyDecimalPrecision(currencySettlementMethod.Term);
            return new ExchangeDaemon()
            {
                Rate = exchangeRate.Rate,
                Precision = currencyPrecision.DecimalPlaces
            };
        }
    }
}
