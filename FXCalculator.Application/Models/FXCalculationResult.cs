namespace FXCalculator.Application.Models
{
    public class FXCalculationResult
    {
        public string BaseCurrency { get; init; }
        public string TermsCurrency { get; init; }
        public decimal OriginalAmount { get; init; }
        public decimal ExchangedAmount { get; init; }
    }
}