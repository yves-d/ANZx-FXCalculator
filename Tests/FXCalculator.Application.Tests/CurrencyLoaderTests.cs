﻿using FluentAssertions;
using FXCalculator.Application.Exceptions;
using System;
using Xunit;

namespace FXCalculator.Application.Tests
{
    public class CurrencyLoaderTests
    {
        private CurrencyLoaderTestHarness _testHarness;

        public CurrencyLoaderTests()
        {
            _testHarness = new CurrencyLoaderTestHarness();
        }

        [Fact]
        public void WHEN_Currency_Is_Not_Found_THEN_GetCurrencySettlementMethod_SHOULD_Throw_CurrencySettlementMethodNotFoundException()
        {
            // arrange
            _testHarness
                .WithNullCurrencySettlementMethod()
                .BuildTestCase();

            // act
            Action act = () => _testHarness.Execute_GetCurrencySettlementMethod();

            // assert
            act.Should().Throw<CurrencySettlementMethodNotFoundException>()
                .WithMessage(_testHarness.CurrencySettlementMethodNotFoundExceptionMessage);
        }

        [Fact]
        public void WHEN_Currency_Pair_Rate_Is_Not_Found_THEN_GetCurrencyPairExchangeRate_SHOULD_Throw_CurrencyPairExchangeRateNotFoundException()
        {
            // arrange
            _testHarness
                .WithNullCurrencyPairExchangeRate()
                .BuildTestCase();

            // act
            Action act = () => _testHarness.Execute_GetCurrencyPairExchangeRate();

            // assert
            act.Should().Throw<CurrencyPairExchangeRateNotFoundException>()
                .WithMessage(_testHarness.CurrencyPairExchangeRateNotFoundExceptionMessage);
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