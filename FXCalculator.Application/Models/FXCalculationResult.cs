namespace FXCalculator.Application.Models
{
    public class FXCalculationResult
    {
        public string BaseCurrency { get; set; }
        public string TermsCurrency { get; set; }
        public decimal ExchangeRate { get; set; }
        public FXCalculationOutcomeEnum Outcome { get; set; }
    }
}
