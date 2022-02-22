using FXCalculator.Application.Interfaces;
using FXCalculator.Common.Models;
using FXCalculator.Data.Interfaces;

namespace FXCalculator.Application.CurrencyExchangers
{
    public class DirectExchanger : IExchangeCurrency
    {
        protected readonly ICurrencyRepository _currencyRepository;

        public DirectExchanger(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public decimal Exchange(decimal amount)
        {
            throw new NotImplementedException();
        }

        public ExchangeInstrument GetExchangeInstrument(CurrencySettlementMethod currencySettlementMethod)
        {
            var currencyPair = _currencyRepository.GetCurrencyPairExchangeRate(currencySettlementMethod.Base, currencySettlementMethod.Term);
            var currencyPrecision = _currencyRepository.GetCurrencyDecimalPrecision(currencySettlementMethod.Term);
            return new ExchangeInstrument()
            {
                Rate = currencyPair.Rate,
                Precision = currencyPrecision.DecimalPlaces
            };
        }
    }
}
