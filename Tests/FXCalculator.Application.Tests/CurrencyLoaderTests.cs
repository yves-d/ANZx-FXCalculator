using FluentAssertions;
using FXCalculator.Application.Exceptions;
using System;
using Xunit;

namespace FXCalculator.Application.Tests
{
    public class CurrencyLoaderTests
    {
        CurrencyLoaderTestHarness _testHarness;

        // test data
        private const string CROSS_METHOD_NOT_FOUND_EXCEPTION_MESSAGE_BTCUSD = "Could not find cross-via method for base 'BTC' and term 'USD'";
        private const string CURRENCY_PAIR_NOT_FOUND_EXCEPTION_MESSAGE_BTCUSD = "Currency pair not found for base 'BTC' and term 'USD'";

        public CurrencyLoaderTests()
        {
            _testHarness = new CurrencyLoaderTestHarness();
        }

        [Fact]
        public void WHEN_Currency_Is_Not_Found_THEN_GetCurrencySettlementMethod_SHOULD_Throw_CrossViaMethodNotFoundException()
        {
            // arrange
            _testHarness
                .WithNullCurrencySettlementMethod()
                .BuildTestCase();

            // act
            Action act = () => _testHarness.Execute_GetCurrencySettlementMethod();

            // assert
            act.Should().Throw<CrossViaMethodNotFoundException>()
                .WithMessage(CROSS_METHOD_NOT_FOUND_EXCEPTION_MESSAGE_BTCUSD);
        }

        [Fact]
        public void WHEN_Currency_Pair_Rate_Is_Not_Found_THEN_GetCurrencyPairExchangeRate_SHOULD_Throw_CurrencyPairNotFoundException()
        {
            // arrange
            _testHarness
                .WithNullCurrencyPairExchangeRate()
                .BuildTestCase();

            // act
            Action act = () => _testHarness.Execute_GetCurrencyPairExchangeRate();

            // assert
            act.Should().Throw<CurrencyPairNotFoundException>()
                .WithMessage(CURRENCY_PAIR_NOT_FOUND_EXCEPTION_MESSAGE_BTCUSD);
        }

        [Fact]
        public void WHEN_SettlementMethod_Term_Parameter_Is_Equal_To_Returned_Base_From_Repository_THEN_GetCurrencySettlementMethod_SHOULD_Return_CurrencySettlementMethod_With_Matching_Term()
        {
            // arrange
            _testHarness
                .WithSettlementTermMatchingBaseFromRepository()
                .BuildTestCase();

            // act
            var currencySettlementMethodResult = _testHarness.Execute_GetCurrencySettlementMethod();

            // assert
            currencySettlementMethodResult.Term.Should().Be(_testHarness.Term);
        }

        [Fact]
        public void WHEN_CurrencyPair_Term_Parameter_Is_Equal_To_Returned_Base_From_Repository_THEN_GetCurrencyPairExchangeRate_SHOULD_Return_CurrencyPairExchangeRate_With_Matching_Term()
        {
            // arrange
            _testHarness
                .WithExchangePairTermMatchingBaseFromRepository()
                .BuildTestCase();

            // act
            var currencyPairExchangeRateResult = _testHarness.Execute_GetCurrencyPairExchangeRate();

            // assert
            currencyPairExchangeRateResult.Term.Should().Be(_testHarness.Term);
        }
    }
}
