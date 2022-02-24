namespace FXCalculator.Application.Exceptions
{
    public class CrossCurrencyNotFoundException : Exception
    {
        public CrossCurrencyNotFoundException()
        {
        }

        public CrossCurrencyNotFoundException(string message)
            : base(message)
        {
        }

        public CrossCurrencyNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
