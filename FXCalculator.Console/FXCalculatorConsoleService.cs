using FXCalculator.Application.Exceptions;
using FXCalculator.Application.Interfaces;
using FXCalculator.Common.Logger;
using FXCalculator.Console.Interfaces;
using FXCalculator.Console.Models;
using System.Text.RegularExpressions;

namespace FXCalculator.Console
{
    public class FXCalculatorConsoleService : IFXCalculatorConsoleService
    {
        private readonly ILoggerAdapter<IFXCalculatorConsoleService> _logger;
        private readonly IFXCalculatorService _fxCalculatorService;

        private const string USER_INPUT_REGEX_VALIDATION = @"^[A-Z]+\s[0-9]*\.?[0-9]+\s(IN)+\s[A-Z]+$";
        private const string USER_INPUT_IS_INVALID_MESSAGE = "Input is invalid! Please try again.";
        private const string VALID_CURRENCY_EXCHANGE_RESPONSE = "{0} {1} = {2} {3}";

        public FXCalculatorConsoleService(ILoggerAdapter<IFXCalculatorConsoleService> logger, IFXCalculatorService fxCalculatorService)
        {
            _logger = logger;
            _fxCalculatorService = fxCalculatorService;
        }

        public string GetCurrencyConversionResponse(string userInput)
        {
            userInput = userInput.Trim().ToUpper();

            if (InputIsInvalid(userInput))
                return USER_INPUT_IS_INVALID_MESSAGE;

            var currencyExchangeRequest = GetCurrencyExchangeRequest(userInput);

            try
            {
                var fxCalculationResult = _fxCalculatorService.CalculateExchangeAmount(
                    currencyExchangeRequest.Base,
                    currencyExchangeRequest.Term,
                    currencyExchangeRequest.Amount);

                return string.Format(
                    VALID_CURRENCY_EXCHANGE_RESPONSE,
                    fxCalculationResult.BaseCurrency,
                    fxCalculationResult.OriginalAmount,
                    fxCalculationResult.TermsCurrency,
                    fxCalculationResult.ExchangedAmount);
            }
            catch(Exception ex)
            {
                return GetExceptionMessageForUser(ex);
            }
        }

        private bool InputIsInvalid(string userInput)
        {
            Regex userInputRegex = new Regex(USER_INPUT_REGEX_VALIDATION);
            return !userInputRegex.IsMatch(userInput);
        }

        private CurrencyExchangeRequest GetCurrencyExchangeRequest(string userInput)
        {
            var individualStrings = userInput.Replace(" IN", "").Split(" ");
            decimal amount;
            decimal.TryParse(individualStrings[1], out amount);

            return new CurrencyExchangeRequest()
            {
                Base = individualStrings[0],
                Amount = amount,
                Term = individualStrings[2]
            };
        }

        private string GetExceptionMessageForUser(Exception ex)
        {
            string error = "";
            switch (ex.GetType().Name)
            {
                case nameof(CurrencySettlementMethodNotFoundException):
                    error = (ex as CurrencySettlementMethodNotFoundException).Message;
                    break;
                case nameof(CurrencyPairExchangeRateNotFoundException):
                    error = (ex as CurrencyPairExchangeRateNotFoundException).Message;
                    break;
                case nameof(UnableToCrossToTermCurrencyException):
                    error = (ex as UnableToCrossToTermCurrencyException).Message;
                    break;
                default:
                    {
                        // if not appropriate to return the exception to the user,
                        // log it instead
                        _logger.LogError(ex.Message);
                        error = "An uknown error has occurred. Please try again.";
                    }
                    break;
            }

            return error;
        }
    }
}

