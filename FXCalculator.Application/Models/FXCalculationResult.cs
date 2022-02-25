namespace FXCalculator.Application.Models
{
    public class FXCalculationResult
    {
        public string BaseCurrency { get; init; }
        public string TermsCurrency { get; init; }
        public decimal ExchangedAmount { get; init; }
        public FXCalculationOutcomeEnum Outcome { get; init; }
    }
}