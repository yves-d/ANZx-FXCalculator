using FXCalculator.Application.Interfaces;
using FXCalculator.Common.Models;

namespace FXCalculator.Application.CurrencyExchangers
{
    public class OneToOneExchanger : IExchangeCurrency
    {
        private readonly ICurrencyLoader _currencyLoader;

        public OneToOneExchanger(ICurrencyLoader currencyLoader)
        {
            _currencyLoader = currencyLoader;
        }

        public ExchangeInstrument GetExchangeInstrument(CurrencySettlementMethod currencySettlementMethod)
        {
            var currencyPrecision = _currencyLoader.GetCurrencyDecimalPrecision(currencySettlementMethod.Base);
            return new ExchangeInstrument()
            {
                Rate = 1.00m,
                Precision = currencyPrecision.DecimalPlaces
            };
        }
    }
}
