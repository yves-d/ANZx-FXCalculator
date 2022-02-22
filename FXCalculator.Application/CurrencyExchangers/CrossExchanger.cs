using FXCalculator.Application.Exceptions;
using FXCalculator.Application.Interfaces;
using FXCalculator.Common.Models;
using FXCalculator.Data.Interfaces;

namespace FXCalculator.Application.CurrencyExchangers
{
    public class CrossExchanger : IExchangeCurrency
    {
        protected readonly ICurrencyRepository _currencyRepository;

        public CrossExchanger(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public decimal Exchange(decimal amount)
        {
            throw new NotImplementedException();
        }

        public ExchangeInstrument GetExchangeInstrument(CurrencySettlementMethod currencySettlementMethod)
        {
            bool stillArrivingBaseAtCrossCurrency = true;
            decimal rateFromBaseToCross = 1;
            CurrencySettlementMethod nextCurrencySettlement = _currencyRepository.GetCurrencySettlementMethod(currencySettlementMethod.Base, currencySettlementMethod.SettlementCurrency);
            while (stillArrivingBaseAtCrossCurrency)
            {
                if (nextCurrencySettlement.SettlementMethod == SettlementMethodEnum.Cross)
                    nextCurrencySettlement = _currencyRepository.GetCurrencySettlementMethod(nextCurrencySettlement.SettlementCurrency, currencySettlementMethod.Term);

                if (nextCurrencySettlement.SettlementMethod != SettlementMethodEnum.Cross)
                {
                    var baseToCrossRate = GetExchangeRate(nextCurrencySettlement);

                    rateFromBaseToCross *= baseToCrossRate;

                    if (currencySettlementMethod.Term == nextCurrencySettlement.Term)
                        stillArrivingBaseAtCrossCurrency = false;
                    else
                        nextCurrencySettlement = _currencyRepository.GetCurrencySettlementMethod(nextCurrencySettlement.Term, currencySettlementMethod.Term);
                }
            }

            var currencyPrecision = _currencyRepository.GetCurrencyDecimalPrecision(currencySettlementMethod.Term);
            return new ExchangeInstrument()
            {
                Rate = rateFromBaseToCross,
                Precision = currencyPrecision.DecimalPlaces
            };
        }

        //public ExchangeInstrument GetExchangeInstrument(CurrencySettlementMethod currencySettlementMethod)
        //{
        //    var baseToCrossSettlementMethod = _currencyRepository.GetCurrencySettlementMethod(currencySettlementMethod.Base, currencySettlementMethod.SettlementCurrency);
        //    var baseToCrossRate = GetExchangeRate(baseToCrossSettlementMethod);

        //    var crossToTermSettlementMethod = _currencyRepository.GetCurrencySettlementMethod(currencySettlementMethod.SettlementCurrency, currencySettlementMethod.Term);
        //    var crossToTermRate = GetExchangeRate(crossToTermSettlementMethod);

        //    var derivedCrossRate = baseToCrossRate * crossToTermRate;

        //    var currencyPrecision = _currencyRepository.GetCurrencyDecimalPrecision(currencySettlementMethod.Term);
        //    return new ExchangeInstrument()
        //    {
        //        Rate = derivedCrossRate,
        //        Precision = currencyPrecision.DecimalPlaces
        //    };
        //}

        private decimal GetExchangeRate(CurrencySettlementMethod currencySettlementMethod)
        {
            if (currencySettlementMethod.SettlementMethod == SettlementMethodEnum.Direct)
                return _currencyRepository.GetCurrencyPairExchangeRate(currencySettlementMethod.Base, currencySettlementMethod.Term).Rate;

            if (currencySettlementMethod.SettlementMethod == SettlementMethodEnum.Inverted)
                return 1 / _currencyRepository.GetCurrencyPairExchangeRate(currencySettlementMethod.Term, currencySettlementMethod.Base).Rate;

            throw new InvalidCurrencySettlementMethodException($"Settlement Method " +
                $"'{currencySettlementMethod.SettlementMethod}' is invalid when determining a cross rate for " +
                $"'{currencySettlementMethod.Base}' and '{currencySettlementMethod.Term}'");
        }
    }
}
