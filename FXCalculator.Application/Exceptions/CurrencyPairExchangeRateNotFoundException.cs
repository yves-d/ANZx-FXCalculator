namespace FXCalculator.Application.Exceptions
{
    public class CurrencyPairExchangeRateNotFoundException : Exception
    {
        public CurrencyPairExchangeRateNotFoundException()
        {
        }

        public CurrencyPairExchangeRateNotFoundException(string message)
            : base(message)
        {
        }

        public CurrencyPairExchangeRateNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}