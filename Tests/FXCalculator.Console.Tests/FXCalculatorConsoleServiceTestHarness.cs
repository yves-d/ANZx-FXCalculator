using FXCalculator.Application.Exceptions;
using FXCalculator.Application.Interfaces;
using FXCalculator.Application.Models;
using FXCalculator.Common.Logger;
using FXCalculator.Console.Interfaces;
using NSubstitute;

namespace FXCalculator.Console.Tests
{
    public class FXCalculatorConsoleServiceTestHarness
    {
        private IFXCalculatorConsoleService _fxCalculatorConsoleService;

        // injectables
        public ILoggerAdapter<IFXCalculatorConsoleService> _logger { get; private set; }
        private IFXCalculatorService _fxCalculatorService;

        // test data
        private string _userInput;
        private FXCalculationResult _calculationResult;
        public string ExpectedConsoleOutput { get; private set; }
        public string ExpectedLogMessage { get; private set; }

        public FXCalculatorConsoleServiceTestHarness()
        {
            _logger = Substitute.For<ILoggerAdapter<IFXCalculatorConsoleService>>();
            _fxCalculatorService = Substitute.For<IFXCalculatorService>();
        }

        public FXCalculatorConsoleServiceTestHarness WithUserInput(string userInput)
        {
            _userInput = userInput;
            return this;
        }

        public FXCalculatorConsoleServiceTestHarness WithCalculateExchangeAmountReturningCalculationResult(
            string baseCurrency,
            string termCurrency,
            decimal amountToExchange,
            decimal amountExchanged)
        {
            _calculationResult = new FXCalculationResult()
            {
                BaseCurrency = baseCurrency,
                TermsCurrency = termCurrency,
                OriginalAmount = amountToExchange,
                ExchangedAmount = amountExchanged
            };
            _fxCalculatorService.CalculateExchangeAmount(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<decimal>()).Returns(_calculationResult);
            ExpectedConsoleOutput = string.Format("{0} {1} = {2} {3}", baseCurrency, amountToExchange, termCurrency, amountExchanged);

            return this;
        }

        public FXCalculatorConsoleServiceTestHarness WithCurrencySettlementMethodNotFoundException(string baseCurrency, string termCurrency)
        {
            ExpectedConsoleOutput = $"Could not find currency settlement method for base '{baseCurrency}' and term '{termCurrency}'";
            _fxCalculatorService.CalculateExchangeAmount(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<decimal>())
                .Returns(x => throw new CurrencySettlementMethodNotFoundException(ExpectedConsoleOutput));
            
            return this;
        }
        
        public FXCalculatorConsoleServiceTestHarness WithCurrencyPairExchangeRateNotFoundException(string baseCurrency, string termCurrency)
        {
            ExpectedConsoleOutput = $"Currency pair exchange rate not found for base '{baseCurrency}' and term '{termCurrency}'";
            _fxCalculatorService.CalculateExchangeAmount(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<decimal>())
                .Returns(x => throw new CurrencyPairExchangeRateNotFoundException(ExpectedConsoleOutput));

            return this;
        }

        public FXCalculatorConsoleServiceTestHarness WithCurrencyExchangerNotImplementedException()
        {
            ExpectedConsoleOutput = "An uknown error has occurred. Please try again.";
            ExpectedLogMessage = $"Currency exchanger not implement for settlement method 'None'!";

            _fxCalculatorService.CalculateExchangeAmount(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<decimal>())
                .Returns(x => throw new CurrencyExchangerNotImplementedException(ExpectedLogMessage));

            return this;
        }

        public FXCalculatorConsoleServiceTestHarness WithUnableToCrossToTermCurrencyException(string termCurrency)
        {
            ExpectedConsoleOutput = $"Unable to find cross rates for term currency '{termCurrency}'.";

            _fxCalculatorService.CalculateExchangeAmount(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<decimal>())
                .Returns(x => throw new UnableToCrossToTermCurrencyException(ExpectedConsoleOutput));

            return this;
        }

        public FXCalculatorConsoleServiceTestHarness BuildTestCase()
        {
            _fxCalculatorConsoleService = new FXCalculatorConsoleService(_logger, _fxCalculatorService);
            return this;
        }

        public string Execute_GetCurrencyConversionResponse()
        {
            return _fxCalculatorConsoleService.GetCurrencyConversionResponse(_userInput);
        }
    }
}