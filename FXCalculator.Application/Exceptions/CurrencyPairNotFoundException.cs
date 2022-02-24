namespace FXCalculator.Application.Exceptions
{
    public class CurrencyPairNotFoundException : Exception
    {
        public CurrencyPairNotFoundException()
        {
        }

        public CurrencyPairNotFoundException(string message)
            : base(message)
        {
        }

        public CurrencyPairNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
