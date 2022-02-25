namespace FXCalculator.Application.Exceptions
{
    public class UnableToCrossToTermCurrencyException : Exception
    {
        public UnableToCrossToTermCurrencyException()
        {
        }

        public UnableToCrossToTermCurrencyException(string message)
            : base(message)
        {
        }

        public UnableToCrossToTermCurrencyException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}