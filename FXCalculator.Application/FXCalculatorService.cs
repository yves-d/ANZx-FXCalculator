using FXCalculator.Application.Interfaces;
using FXCalculator.Application.Models;
using FXCalculator.Data.Interfaces;

namespace FXCalculator.Application
{
    public class FXCalculatorService : IFXCalculatorService
    {
        ICurrencyRepository _currencyRepository;

        public FXCalculatorService(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public FXCalculationResult CalculateExchangeAmount(string baseCurrency, string termsCurrency, decimal amount)
        {
            throw new NotImplementedException();
        }
    }
}