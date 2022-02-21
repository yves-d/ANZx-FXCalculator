namespace FXCalculator.Common.Models
{
    public class CurrencySettlementMethod
    {
        public string Base { get; init; }
        public string Term { get; init; }
        public SettlementMethodEnum SettlementMethod { get; init; }
        public string? SettlementCurrency { get; init; }
    }
}
