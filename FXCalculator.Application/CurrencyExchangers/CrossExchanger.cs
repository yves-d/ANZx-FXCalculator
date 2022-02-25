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
            CurrencySettlementMethod nextCurrencySettlementMethod = currencySettlementMethod;
            decimal baseToTermRate = STARTING_RATE;
            bool stillConvertingCurrency = true;
            int currencyHops = 0;

            while (stillConvertingCurrency && currencyHops < MAX_CURRENCY_HOPS)
            {
                if (nextCurrencySettlementMethod.SettlementMethod == SettlementMethodEnum.Cross)
                    nextCurrencySettlementMethod = _currencyLoader.GetCurrencySettlementMethod(nextCurrencySettlementMethod.Base, nextCurrencySettlementMethod.SettlementCurrency);

                if (nextCurrencySettlementMethod.SettlementMethod != SettlementMethodEnum.Cross)
                {
                    var currentBaseToTermRate = _currencyLoader.GetCurrencyPairExchangeRate(nextCurrencySettlementMethod.Base, nextCurrencySettlementMethod.Term);

                    baseToTermRate *= currentBaseToTermRate.Rate;

                    if (currencySettlementMethod.Term == nextCurrencySettlementMethod.Term)
                        stillConvertingCurrency = false;
                    else
                        nextCurrencySettlementMethod = _currencyLoader.GetCurrencySettlementMethod(nextCurrencySettlementMethod.Term, currencySettlementMethod.Term);
                }
                currencyHops++;
            }

            if (currencyHops >= MAX_CURRENCY_HOPS)
                throw new UnableToCrossToTermCurrencyException($"Unable to cross to term currency '{currencySettlementMethod.Term}'. Search reached {currencyHops} hops.");

            var currencyPrecision = _currencyLoader.GetCurrencyDecimalPrecision(currencySettlementMethod.Term);
            return new ExchangeInstrument()
            {
                Rate = baseToTermRate,
                Precision = currencyPrecision.DecimalPlaces
            };
        }
    }
}