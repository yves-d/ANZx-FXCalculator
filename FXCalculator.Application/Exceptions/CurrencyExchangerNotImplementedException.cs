namespace FXCalculator.Application.Exceptions
{
    public class CurrencyExchangerNotImplementedException : Exception
    {
        public CurrencyExchangerNotImplementedException()
        {
        }

        public CurrencyExchangerNotImplementedException(string message)
            : base(message)
        {
        }

        public CurrencyExchangerNotImplementedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
