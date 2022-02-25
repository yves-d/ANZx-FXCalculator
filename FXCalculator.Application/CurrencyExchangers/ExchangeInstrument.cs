namespace FXCalculator.Application.CurrencyExchangers
{
    public class ExchangeInstrument
    {
        public decimal Rate { get; init; }
        public int Precision { get; init; }

        public decimal Exchange(decimal amount)
        {
            return decimal.Round(amount * Rate, Precision);
        }
    }
}