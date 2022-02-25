using FXCalculator.Application.Exceptions;
using FXCalculator.Application.Interfaces;
using FXCalculator.Common.Models;
using FXCalculator.Data.Interfaces;

namespace FXCalculator.Application
{
    public class CurrencyLoader : ICurrencyLoader
    {
        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyLoader(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public CurrencySettlementMethod GetCurrencySettlementMethod(string baseCurrency, string termCurrency)
        {
            var currencySettlementMethod = _currencyRepository.GetCurrencySettlementMethod(baseCurrency, termCurrency);

            if (currencySettlementMethod == null)
                throw new CurrencySettlementMethodNotFoundException($"Could not find currency settlement method for base '{baseCurrency}' and term '{termCurrency}'");

            // swap the base and term around, to complete the inverse 'symmetrical' side of the data set
            if (currencySettlementMethod.Base == termCurrency && currencySettlementMethod.CrossVia != CrossViaEnum.OneToOne)
            {
                return new CurrencySettlementMethod()
                {
                    Base = baseCurrency,
                    Term = termCurrency,
                    SettlementCurrency = currencySettlementMethod.SettlementCurrency,
                    CrossVia = FlipDirectIndirectConversion(currencySettlementMethod.CrossVia)
                };
            }

            return currencySettlementMethod;
        }

        public CurrencyPairExchangeRate GetCurrencyPairExchangeRate(string baseCurrency, string termCurrency)
        {
            var exchangePair = _currencyRepository.GetCurrencyPairExchangeRate(baseCurrency, termCurrency);

            if (exchangePair == null)
                throw new CurrencyPairExchangeRateNotFoundException($"Currency pair exchange rate not found for base '{baseCurrency}' and term '{termCurrency}'");

            // handle inverse pairs
            if(exchangePair.Base == termCurrency && exchangePair.Term == baseCurrency)
            {
                return new CurrencyPairExchangeRate()
                {
                    Base = baseCurrency,
                    Term = termCurrency,
                    Rate = 1 / exchangePair.Rate
                };
            }

            return exchangePair;
        }

        public CurrencyDecimalPrecision GetCurrencyDecimalPrecision(string currency)
        {
            return _currencyRepository.GetCurrencyDecimalPrecision(currency);
        }

        private CrossViaEnum FlipDirectIndirectConversion(CrossViaEnum crossVia)
        {
            if (crossVia == CrossViaEnum.Direct)
                return CrossViaEnum.Inverted;
            else if (crossVia == CrossViaEnum.Inverted)
                return CrossViaEnum.Direct;
            else
                return crossVia;
        }
    }
}