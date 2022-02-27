using FluentAssertions;
using FXCalculator.Application.Exceptions;
using FXCalculator.Application.Interfaces;
using FXCalculator.Application.Models;
using FXCalculator.Console.Interfaces;
using NSubstitute;
using Xunit;

namespace FXCalculator.Console.Tests
{
    public class FXCalculatorConsoleServiceTests
    {
        private IFXCalculatorConsoleService _fxCalculatorConsoleService;
        private IFXCalculatorService _fxCalculatorService;

        public FXCalculatorConsoleServiceTests()
        {
            _fxCalculatorService = Substitute.For<IFXCalculatorService>();
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
            _fxCalculatorConsoleService = new FXCalculatorConsoleService(_fxCalculatorService);

            // act
            var result = _fxCalculatorConsoleService.GetCurrencyConversionResponse(userInput);

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
            var expectedOutput = string.Format("{0} {1} = {2} {3}", baseCurrency, amountToExchange, termCurrency, amountExchanged);
            var calculatedExchangeResponse = new FXCalculationResult()
            {
                BaseCurrency = baseCurrency,
                TermsCurrency = termCurrency,
                OriginalAmount = amountToExchange,
                ExchangedAmount = amountExchanged
            };
            _fxCalculatorService.CalculateExchangeAmount(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<decimal>()).Returns(calculatedExchangeResponse);
            _fxCalculatorConsoleService = new FXCalculatorConsoleService(_fxCalculatorService);

            // act
            var result = _fxCalculatorConsoleService.GetCurrencyConversionResponse(userInput);

            // assert
            result.Should().Be(expectedOutput);
        }

        [Theory]
        [InlineData("AUD 100.00 IN BTC", "AUD", "BTC")]
        public void WHEN_CalculateExchangeAmount_Throws_CurrencySettlementMethodNotFoundException_THEN_GetCurrencyConversionResponse_Should_Return_Appropriate_Error_Message(
            string userInput,
            string baseCurrency,
            string termCurrency)
        {
            // arrange
            var expectedExceptionMessage = $"Could not find currency settlement method for base '{baseCurrency}' and term '{termCurrency}'";
            var calculatedExchangeResponse = new FXCalculationResult()
            {
                BaseCurrency = baseCurrency,
                TermsCurrency = termCurrency,
                OriginalAmount = 100.00m,
                ExchangedAmount = 100.00m
            };
            _fxCalculatorService.CalculateExchangeAmount(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<decimal>())
                .Returns(x => throw new CurrencySettlementMethodNotFoundException());
            _fxCalculatorConsoleService = new FXCalculatorConsoleService(_fxCalculatorService);

            // act
            var result = _fxCalculatorConsoleService.GetCurrencyConversionResponse(userInput);

            // assert
            result.Should().Be(expectedExceptionMessage);
        }

        [Theory]
        [InlineData("AUD 100.00 IN BTC", "AUD", "BTC")]
        public void WHEN_CalculateExchangeAmount_Throws_CurrencyPairExchangeRateNotFoundException_THEN_GetCurrencyConversionResponse_Should_Return_Appropriate_Error_Message(
            string userInput,
            string baseCurrency,
            string termCurrency)
        {
            // arrange
            var expectedExceptionMessage = $"Currency pair exchange rate not found for base '{baseCurrency}' and term '{termCurrency}'";
            var calculatedExchangeResponse = new FXCalculationResult()
            {
                BaseCurrency = baseCurrency,
                TermsCurrency = termCurrency,
                OriginalAmount = 100.00m,
                ExchangedAmount = 100.00m
            };
            _fxCalculatorService.CalculateExchangeAmount(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<decimal>())
                .Returns(x => throw new CurrencyPairExchangeRateNotFoundException());
            _fxCalculatorConsoleService = new FXCalculatorConsoleService(_fxCalculatorService);

            // act
            var result = _fxCalculatorConsoleService.GetCurrencyConversionResponse(userInput);

            // assert
            result.Should().Be(expectedExceptionMessage);
        }

        [Theory]
        [InlineData("AUD 100.00 IN BTC", "AUD", "BTC")]
        public void WHEN_CalculateExchangeAmount_Throws_CurrencyExchangerNotImplementedException_THEN_GetCurrencyConversionResponse_Should_Return_Appropriate_Error_Message(
            string userInput,
            string baseCurrency,
            string termCurrency)
        {
            // arrange
            var expectedExceptionMessage = $"Currency exchanger not implement for settlement method 'None'!";
            var calculatedExchangeResponse = new FXCalculationResult()
            {
                BaseCurrency = baseCurrency,
                TermsCurrency = termCurrency,
                OriginalAmount = 100.00m,
                ExchangedAmount = 100.00m
            };
            _fxCalculatorService.CalculateExchangeAmount(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<decimal>())
                .Returns(x => throw new CurrencyExchangerNotImplementedException());
            _fxCalculatorConsoleService = new FXCalculatorConsoleService(_fxCalculatorService);

            // act
            var result = _fxCalculatorConsoleService.GetCurrencyConversionResponse(userInput);

            // assert
            result.Should().Be(expectedExceptionMessage);
        }

        [Theory]
        [InlineData("AUD 100.00 IN BTC", "AUD", "BTC")]
        public void WHEN_CalculateExchangeAmount_Throws_UnableToCrossToTermCurrencyException_THEN_GetCurrencyConversionResponse_Should_Return_Appropriate_Error_Message(
            string userInput,
            string baseCurrency,
            string termCurrency)
        {
            // arrange
            var expectedExceptionMessage = $"Unable to cross to term currency 'termCurrency'. Search reached 10 hops.";
            var calculatedExchangeResponse = new FXCalculationResult()
            {
                BaseCurrency = baseCurrency,
                TermsCurrency = termCurrency,
                OriginalAmount = 100.00m,
                ExchangedAmount = 100.00m
            };
            _fxCalculatorService.CalculateExchangeAmount(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<decimal>())
                .Returns(x => throw new UnableToCrossToTermCurrencyException());
            _fxCalculatorConsoleService = new FXCalculatorConsoleService(_fxCalculatorService);

            // act
            var result = _fxCalculatorConsoleService.GetCurrencyConversionResponse(userInput);

            // assert
            result.Should().Be(expectedExceptionMessage);
        }
    }
}