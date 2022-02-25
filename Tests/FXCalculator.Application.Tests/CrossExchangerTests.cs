using FluentAssertions;
using FXCalculator.Application.Exceptions;
using System;
using Xunit;

namespace FXCalculator.Application.Tests
{
    public class CrossExchangerTests
    {
        private CrossExchangerTestHarness _testHarness;

        public CrossExchangerTests()
        {
            _testHarness = new CrossExchangerTestHarness();
        }

        [Fact]
        public void WHEN_Term_Rate_Cannot_Be_Found_In_Under_10_Loops_THEN_GetExchangeInstrument_SHOULD_Throw_UnableToCrossToTermCurrencyException()
        {
            // arrange
            _testHarness
                .WithStartingCrossCurrencySettlementMethod("AUD", "JPY", "USD")
                .WithGetCurrencySettlementMethodReflectingStartingSettlementMethod()
                .BuildTestCase();

            // act
            Action act = () => _testHarness.Execute_GetExchangeInstrument();

            // assert
            act.Should().Throw<UnableToCrossToTermCurrencyException>()
                .WithMessage(_testHarness.UnableToCrossToTermCurrencyExceptionMessage);
        }
    }
}