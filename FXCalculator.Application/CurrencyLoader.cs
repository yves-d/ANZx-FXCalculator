using FXCalculator.Application.Interfaces;
using FXCalculator.Common.Models;
using FXCalculator.Data.Exceptions;
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
                throw new SettlementMethodNotFoundException($"Could not find settlement method for base '{baseCurrency}' and term '{termCurrency}'");

            // swap the base and term around, to represent the complete table
            if (currencySettlementMethod.Base == termCurrency && currencySettlementMethod.SettlementMethod != SettlementMethodEnum.OneToOne)
            {
                return new CurrencySettlementMethod()
                {
                    Base = baseCurrency,
                    Term = termCurrency,
                    SettlementCurrency = currencySettlementMethod.SettlementCurrency,
                    SettlementMethod = FlipDirectIndirectConversion(currencySettlementMethod.SettlementMethod)
                };
            }

            return currencySettlementMethod;
        }

        public CurrencyPairExchangeRate GetCurrencyPairExchangeRate(string baseCurrency, string termCurrency)
        {
            //return _currencyPairExchangeRates.SingleOrDefault(pair => pair.Base == baseCurrency && pair.Term == termCurrency);
            return _currencyRepository.GetCurrencyPairExchangeRate(baseCurrency, termCurrency);
        }

        public CurrencyDecimalPrecision GetCurrencyDecimalPrecision(string currency)
        {
            return _currencyRepository.GetCurrencyDecimalPrecision(currency);
        }

        private SettlementMethodEnum FlipDirectIndirectConversion(SettlementMethodEnum savedSettlementMethod)
        {
            if (savedSettlementMethod == SettlementMethodEnum.Direct)
                return SettlementMethodEnum.Inverted;
            else if (savedSettlementMethod == SettlementMethodEnum.Inverted)
                return SettlementMethodEnum.Direct;
            else
                return savedSettlementMethod;
        }
    }
}
