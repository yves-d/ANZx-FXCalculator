using FXCalculator.Application.Interfaces;
using FXCalculator.Application.Models;

namespace FXCalculator.Application
{
    public class FXCalculatorService : IFXCalculatorService
    {
        private readonly ICurrencyLoader _currencyLoader;
        private readonly ICurrencyExchangeFactory _currencyExchangeFactory;

        public FXCalculatorService(ICurrencyLoader currencyLoader, ICurrencyExchangeFactory currencyExchangeFactory)
        {
            _currencyLoader = currencyLoader;
            _currencyExchangeFactory = currencyExchangeFactory; 
        }

        public FXCalculationResult CalculateExchangeAmount(string baseCurrency, string termsCurrency, decimal amount)
        {
            var currencySettlementMethod = _currencyLoader.GetCurrencySettlementMethod(baseCurrency, termsCurrency);

            var currencyExchanger = _currencyExchangeFactory.GetCurrencyExchanger(currencySettlementMethod.SettlementMethod);

            var exchangeInstrument = currencyExchanger.GetExchangeInstrument(currencySettlementMethod);

            var exchangedAmount = exchangeInstrument.Exchange(amount);

            return CreateNewFXCalculationResult(baseCurrency, termsCurrency, exchangedAmount, FXCalculationOutcomeEnum.Success);
        }

        private FXCalculationResult CreateNewFXCalculationResult(string baseCurrency, string termsCurrency, decimal amount, FXCalculationOutcomeEnum outcome)
        {
            return new FXCalculationResult()
            {
                BaseCurrency = baseCurrency,
                TermsCurrency = termsCurrency,
                ExchangedAmount = amount,
                Outcome = outcome
            };
        }
    }
}