using FXCalculator.Application.Interfaces;
using FXCalculator.Common.Models;

namespace FXCalculator.Application.CurrencyExchangers
{
    public class DirectExchanger : IExchangeCurrency
    {
        private readonly ICurrencyLoader _currencyLoader;

        public DirectExchanger(ICurrencyLoader currencyLoader)
        {
            _currencyLoader = currencyLoader;
        }

        public ExchangeInstrument GetExchangeInstrument(CurrencySettlementMethod currencySettlementMethod)
        {
            var currencyPair = _currencyLoader.GetCurrencyPairExchangeRate(currencySettlementMethod.Base, currencySettlementMethod.Term);
            var currencyPrecision = _currencyLoader.GetCurrencyDecimalPrecision(currencySettlementMethod.Term);
            return new ExchangeInstrument()
            {
                Rate = currencyPair.Rate,
                Precision = currencyPrecision.DecimalPlaces
            };
        }
    }
}
