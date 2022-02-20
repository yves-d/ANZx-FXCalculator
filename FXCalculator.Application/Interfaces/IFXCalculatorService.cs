using FXCalculator.Application.Models;

namespace FXCalculator.Application.Interfaces
{
    public interface IFXCalculatorService
    {
        FXCalculationResult CalculateExchangeAmount(string baseCurrency, string termsCurrency, decimal amount);
    }
}
