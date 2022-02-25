using FXCalculator.Application.Interfaces;
using FXCalculator.Common.Models;
using FXCalculator.Data.Interfaces;
using NSubstitute;

namespace FXCalculator.Application.Tests
{
    public  class CurrencyLoaderTestHarness
    {
        private ICurrencyLoader _currencyLoader;

        // injectables
        private ICurrencyRepository _currencyRepository;

        // test data
        public string Base { get; private set; } = "";
        public string Term { get; private set; } = "";
        public string CurrencySettlementMethodNotFoundExceptionMessage => $"Could not find currency settlement method for base '{Base}' and term '{Term}'";
        public string CurrencyPairExchangeRateNotFoundExceptionMessage => $"Currency pair exchange rate not found for base '{Base}' and term '{Term}'";

        public CurrencyLoaderTestHarness()
        {
            _currencyRepository = Substitute.For<ICurrencyRepository>();
        }

        public CurrencyLoaderTestHarness WithNullCurrencySettlementMethod()
        {
            Base = "BTC";
            Term = "USD";

            _currencyRepository.GetCurrencySettlementMethod(Arg.Any<string>(), Arg.Any<string>()).Returns((CurrencySettlementMethod)null);
            return this;
        }

        public CurrencyLoaderTestHarness WithNullCurrencyPairExchangeRate()
        {
            Base = "BTC";
            Term = "USD";

            _currencyRepository.GetCurrencyPairExchangeRate(Arg.Any<string>(), Arg.Any<string>()).Returns((CurrencyPairExchangeRate)null);
            return this;
        }

        public CurrencyLoaderTestHarness WithSettlementTermMatchingBaseFromRepository()
        {
            Base = "AUD";
            Term = "USD";

            var currencySettlementMethod = new CurrencySettlementMethod()
            {
                Base = Term,
                Term = Base,
                CrossVia = CrossViaEnum.Direct,
                SettlementCurrency = null
            };
            _currencyRepository.GetCurrencySettlementMethod(Arg.Any<string>(), Arg.Any<string>()).Returns(currencySettlementMethod);
            
            return this;
        }

        public CurrencyLoaderTestHarness WithExchangePairTermMatchingBaseFromRepository()
        {
            Base = "AUD";
            Term = "USD";

            var currencyPairExchangeRate = new CurrencyPairExchangeRate()
            {
                Base = Term,
                Term = Base,
                Rate = 1
            };
            _currencyRepository.GetCurrencyPairExchangeRate(Arg.Any<string>(), Arg.Any<string>()).Returns(currencyPairExchangeRate);

            return this;
        }

        public CurrencyLoaderTestHarness BuildTestCase()
        {
            _currencyLoader = new CurrencyLoader(_currencyRepository);
            return this;
        }

        public CurrencySettlementMethod Execute_GetCurrencySettlementMethod()
        {
            return _currencyLoader.GetCurrencySettlementMethod(Base, Term);
        }

        public CurrencyPairExchangeRate Execute_GetCurrencyPairExchangeRate()
        {
            return _currencyLoader.GetCurrencyPairExchangeRate(Base, Term);
        }
    }
}