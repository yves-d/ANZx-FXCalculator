namespace FXCalculator.Application.Exceptions
{
    public class InvalidCurrencySettlementMethodException : Exception
    {
        public InvalidCurrencySettlementMethodException()
        {
        }

        public InvalidCurrencySettlementMethodException(string message)
            : base(message)
        {
        }

        public InvalidCurrencySettlementMethodException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
