using FXCalculator.Application.Interfaces;
using FXCalculator.Application.Models;
using FXCalculator.Data.Interfaces;

namespace FXCalculator.Application
{
    public class FXCalculatorService : IFXCalculatorService
    {
        ICurrencyRepository _currencyRepository;
        ICurrencyExchangeFactory _currencyExchangeFactory;

        public FXCalculatorService(ICurrencyRepository currencyRepository, ICurrencyExchangeFactory currencyExchangeFactory)
        {
            _currencyRepository = currencyRepository;
            _currencyExchangeFactory = currencyExchangeFactory; 
        }

        public FXCalculationResult CalculateExchangeAmount(string baseCurrency, string termsCurrency, decimal amount)
        {
            var currencySettlementMethod = _currencyRepository.GetCurrencySettlementMethod(baseCurrency, termsCurrency);

            var currencyExchanger = _currencyExchangeFactory.GetCurrencyExchanger(currencySettlementMethod.SettlementMethod);

            var exchangeInstrument = currencyExchanger.GetExchangeInstrument(currencySettlementMethod);

            var exchangedAmount = exchangeInstrument.Exchange(amount);

            return new FXCalculationResult()
            {
                BaseCurrency = baseCurrency,
                TermsCurrency = termsCurrency, 
                ExchangedAmount = exchangedAmount,
                Outcome = FXCalculationOutcomeEnum.SUCCESS
            };
        }
    }
}