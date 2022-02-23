using FXCalculator.Application.Interfaces;
using FXCalculator.Common.Models;
using FXCalculator.Data.Interfaces;

namespace FXCalculator.Application.CurrencyExchangers
{
    public class InvertedExchanger : IExchangeCurrency
    {
        private readonly ICurrencyLoader _currencyLoader;

        public InvertedExchanger(ICurrencyLoader currencyLoader)
        {
            _currencyLoader = currencyLoader;
        }

        public decimal Exchange(decimal amount)
        {
            throw new NotImplementedException();
        }

        public ExchangeInstrument GetExchangeInstrument(CurrencySettlementMethod currencySettlementMethod)
        {
            var exchangeRate = _currencyLoader.GetCurrencyPairExchangeRate(currencySettlementMethod.Term, currencySettlementMethod.Base);
            var currencyPrecision = _currencyLoader.GetCurrencyDecimalPrecision(currencySettlementMethod.Base);
            return new ExchangeInstrument()
            {
                Rate = 1 / exchangeRate.Rate,
                Precision = currencyPrecision.DecimalPlaces
            };
        }
    }
}
