using FluentAssertions;
using NSubstitute;
using Xunit;

namespace FXCalculator.Console.Tests
{
    public class FXCalculatorConsoleServiceTests
    {
        private FXCalculatorConsoleServiceTestHarness _testHarness;

        public FXCalculatorConsoleServiceTests()
        {
            _testHarness = new FXCalculatorConsoleServiceTestHarness();
        }

        [Theory]
        [InlineData("AUD")]
        [InlineData("AUD100INUSD")]
        [InlineData("AUD 100INUSD")]
        [InlineData("AUD100 INUSD")]
        [InlineData("AUD100IN USD")]
        [InlineData("AU D100 IN USD")]
        [InlineData("AUD 100I N USD")]
        [InlineData("AUD USD 100 IN USD")]
        [InlineData("AUD 100 IN USD USD")]
        [InlineData("AUD 100.00. IN USD")]
        [InlineData("AUD 100.00.1 IN USD")]
        public void WHEN_User_Input_Does_Not_Match_Expected_Format_THEN_GetCurrencyConversionResponse_Should_Return_Validation_Error(
            string userInput)
        {
            // arrange
            _testHarness
                .WithUserInput(userInput)
                .BuildTestCase();

            // act
            var result = _testHarness.Execute_GetCurrencyConversionResponse();

            // assert
            result.Should().Be("Input is invalid! Please try again.");
        }

        [Theory]
        [InlineData("AUD 100 IN USD", "AUD", "USD", 100.00, 83.71)]
        [InlineData("AUD 100.00 IN USD", "AUD", "USD", 100.00, 83.71)]
        [InlineData("USD 100.00 in AUD", "USD", "AUD", 100.00, 119.46)]
        public void WHEN_User_Input_Is_Correct_Format_THEN_GetCurrencyConversionResponse_Should_Return_Valid_Exchange_Result(
            string userInput,
            string baseCurrency,
            string termCurrency,
            decimal amountToExchange,
            decimal amountExchanged)
        {
            // arrange
            _testHarness
                .WithUserInput(userInput)
                .WithCalculateExchangeAmountReturningCalculationResult(baseCurrency, termCurrency, amountToExchange, amountExchanged)
                .BuildTestCase();

            // act
            var result = _testHarness.Execute_GetCurrencyConversionResponse();

            // assert
            result.Should().Be(_testHarness.ExpectedConsoleOutput);
        }

        [Fact]
        public void WHEN_CalculateExchangeAmount_Throws_CurrencySettlementMethodNotFoundException_THEN_GetCurrencyConversionResponse_Should_Return_Appropriate_Error_Message()
        {
            // arrange
            _testHarness
                .WithUserInput("AUD 100.00 IN BTC")
                .WithCurrencySettlementMethodNotFoundException("AUD", "BTC")
                .BuildTestCase();

            // act
            var result = _testHarness.Execute_GetCurrencyConversionResponse();

            // assert
            result.Should().Be(_testHarness.ExpectedConsoleOutput);
        }

        [Fact]
        public void WHEN_CalculateExchangeAmount_Throws_CurrencyPairExchangeRateNotFoundException_THEN_GetCurrencyConversionResponse_Should_Return_Appropriate_Error_Message()
        {
            // arrange
            _testHarness
                .WithUserInput("AUD 100.00 IN BTC")
                .WithCurrencyPairExchangeRateNotFoundException("AUD", "BTC")
                .BuildTestCase();

            // act
            var result = _testHarness.Execute_GetCurrencyConversionResponse();

            // assert
            result.Should().Be(_testHarness.ExpectedConsoleOutput);
        }

        [Fact]
        public void WHEN_CalculateExchangeAmount_Throws_CurrencyExchangerNotImplementedException_THEN_GetCurrencyConversionResponse_Should_Return_Generic_Error_Message_And_Log_Exception_Message()
        {
            // arrange
            _testHarness
                .WithUserInput("AUD 100.00 IN BTC")
                .WithCurrencyExchangerNotImplementedException()
                .BuildTestCase();

            // act
            var result = _testHarness.Execute_GetCurrencyConversionResponse();

            // assert
            result.Should().Be(_testHarness.ExpectedConsoleOutput);
            _testHarness._logger.Received(1).LogError(_testHarness.ExpectedLogMessage);
        }

        [Fact]
        public void WHEN_CalculateExchangeAmount_Throws_UnableToCrossToTermCurrencyException_THEN_GetCurrencyConversionResponse_Should_Return_Appropriate_Error_Message()
        {
            // arrange
            _testHarness
                .WithUserInput("AUD 100.00 IN BTC")
                .WithUnableToCrossToTermCurrencyException("AUD")
                .BuildTestCase();

            // act
            var result = _testHarness.Execute_GetCurrencyConversionResponse();

            // assert
            result.Should().Be(_testHarness.ExpectedConsoleOutput);
        }
    }
}