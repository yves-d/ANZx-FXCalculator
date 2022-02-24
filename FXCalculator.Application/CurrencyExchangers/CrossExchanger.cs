using FXCalculator.Application.Exceptions;
using FXCalculator.Application.Interfaces;
using FXCalculator.Common.Models;

namespace FXCalculator.Application.CurrencyExchangers
{
    public class CrossExchanger : IExchangeCurrency
    {
        private readonly ICurrencyLoader _currencyLoader;
        private const int MAX_CURRENCY_HOPS = 10;
        private const decimal STARTING_RATE = 1.00m;

        public CrossExchanger(ICurrencyLoader currencyLoader)
        {
            _currencyLoader = currencyLoader;
        }

        public ExchangeInstrument GetExchangeInstrument(CurrencySettlementMethod currencySettlementMethod)
        {
            bool stillConvertingCurrency = true;
            int currencyHops = 0;
            decimal rateFromBaseToCross = STARTING_RATE;
            CurrencySettlementMethod nextCurrencySettlement = currencySettlementMethod;
            while (stillConvertingCurrency && currencyHops < MAX_CURRENCY_HOPS)
            {
                if (nextCurrencySettlement.SettlementMethod == SettlementMethodEnum.Cross)
                    nextCurrencySettlement = _currencyLoader.GetCurrencySettlementMethod(nextCurrencySettlement.Base, nextCurrencySettlement.SettlementCurrency);

                if (nextCurrencySettlement.SettlementMethod != SettlementMethodEnum.Cross)
                {
                    var baseToCrossRate = _currencyLoader.GetCurrencyPairExchangeRate(nextCurrencySettlement.Base, nextCurrencySettlement.Term);

                    rateFromBaseToCross *= baseToCrossRate.Rate;

                    if (currencySettlementMethod.Term == nextCurrencySettlement.Term)
                        stillConvertingCurrency = false;
                    else
                        nextCurrencySettlement = _currencyLoader.GetCurrencySettlementMethod(nextCurrencySettlement.Term, currencySettlementMethod.Term);
                }
                currencyHops++;
            }

            if (currencyHops >= MAX_CURRENCY_HOPS)
                throw new CrossCurrencyNotFoundException($"Unable to find settlement currency to cross with. Search reached {currencyHops} hops.");

            var currencyPrecision = _currencyLoader.GetCurrencyDecimalPrecision(currencySettlementMethod.Term);
            return new ExchangeInstrument()
            {
                Rate = rateFromBaseToCross,
                Precision = currencyPrecision.DecimalPlaces
            };
        }
    }
}
