namespace FXCalculator.Application.CurrencyExchangers
{
    public class ExchangeInstrument
    {
        public decimal Rate { get; init; }
        public int Precision { get; set; }

        public decimal Exchange(decimal amount)
        {
            var exchangedAmount = amount * Rate;
            return decimal.Round(exchangedAmount, Precision);
        }
    }
}
