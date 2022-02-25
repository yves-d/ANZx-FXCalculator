using FXCalculator.Application.CurrencyExchangers;
using FXCalculator.Application.Interfaces;
using FXCalculator.Common.Models;
using NSubstitute;

namespace FXCalculator.Application.Tests
{
    public class CrossExchangerTestHarness
    {
        private IExchangeCurrency _currencyExchanger;
        
        // injectables
        private ICurrencyLoader _currencyLoader;

        // test data
        private CurrencySettlementMethod startingCurrencySettlementMethod;
        public string UnableToCrossToTermCurrencyExceptionMessage => $"Unable to cross to term currency '{startingCurrencySettlementMethod.Term}'. Search reached 10 hops.";

        public CrossExchangerTestHarness()
        {
            _currencyLoader = Substitute.For<ICurrencyLoader>();
        }

        public CrossExchangerTestHarness WithStartingCrossCurrencySettlementMethod(string baseCurrency, string termCurrency, string settlementCurrency)
        {
            startingCurrencySettlementMethod = new CurrencySettlementMethod()
            {
                Base = baseCurrency,
                Term = termCurrency,
                CrossVia = CrossViaEnum.Cross,
                SettlementCurrency = settlementCurrency
            };

            return this;
        }

        public CrossExchangerTestHarness WithGetCurrencySettlementMethodReflectingStartingSettlementMethod()
        {
            var repeatCurrencySettlementMethod = new CurrencySettlementMethod()
            {
                Base = startingCurrencySettlementMethod.Term,
                Term = startingCurrencySettlementMethod.Base,
                CrossVia = CrossViaEnum.Cross,
                SettlementCurrency = startingCurrencySettlementMethod.SettlementCurrency
            };
            _currencyLoader.GetCurrencySettlementMethod(Arg.Any<string>(), Arg.Any<string>()).Returns(repeatCurrencySettlementMethod);

            return this;
        }

        public CrossExchangerTestHarness BuildTestCase()
        {
            _currencyExchanger = new CrossExchanger(_currencyLoader);
            return this;
        }

        public ExchangeInstrument Execute_GetExchangeInstrument()
        {
            return _currencyExchanger.GetExchangeInstrument(startingCurrencySettlementMethod);
        }
    }
}