namespace FXCalculator.Common.Models
{
    public class CurrencySettlementMethod
    {        
        public string Base { get; init; }
        public string Term { get; init; }
        public CrossViaEnum CrossVia { get; init; }
        public SettlementMethodEnum SettlementMethod => GetSettlementMethod();
        public string? SettlementCurrency { get; init; }

        private SettlementMethodEnum GetSettlementMethod()
        {
            switch(CrossVia)
            {
                case CrossViaEnum.Direct: return SettlementMethodEnum.Direct;
                case CrossViaEnum.Inverted:  return SettlementMethodEnum.Direct;
                case CrossViaEnum.OneToOne: return SettlementMethodEnum.OneToOne;
                case CrossViaEnum.Cross: return SettlementMethodEnum.Cross;
                default: return SettlementMethodEnum.None;
            }
        }
    }
}