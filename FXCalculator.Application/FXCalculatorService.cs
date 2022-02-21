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
            var settlementMethod = _currencyRepository.GetCurrencySettlementMethod(baseCurrency, termsCurrency);

            var currencyExchanger = _currencyExchangeFactory.GetCurrencyExchanger(settlementMethod.SettlementMethod);

            var exchangeDaemon = currencyExchanger.GetExchangeDaemon(settlementMethod);

            var exchangedAmount = exchangeDaemon.Exchange(amount);

            return new FXCalculationResult()
            {
                BaseCurrency = baseCurrency,
                TermsCurrency = termsCurrency, 
                ExchangedAmount = exchangedAmount,
                Outcome = FXCalculationOutcomeEnum.SUCCESS
            };

            throw new NotImplementedException();
        }
    }
}